using DPA_Musicsheets.Managers;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Parsers
{
    class MidiParser : IParser
    {
        private int _beatNote = 4;    // De waarde van een beatnote.
        private int _bpm = 120;       // Aantal beatnotes per minute.
        private int _beatsPerBar;     // Aantal beatnotes per maat.
        private int _division;
        private int _previousMidiKey = 60; // Central C;
        private int _previousNoteAbsoluteTicks = 0;
        private double _percentageOfBarReached = 0;
        private bool _startedNoteIsClosed = true;

        public string Parse(Sequence sequence)
        {
            StringBuilder lilypondContent = new StringBuilder();
            List<string> stringList = new List<string>();
            lilypondContent.AppendLine("\\relative c' {");
            lilypondContent.AppendLine("\\clef treble");
            System.Diagnostics.Debug.WriteLine(sequence);
            _division = sequence.Division;

            for (int i = 0; i < sequence.Count(); i++)
            {
                Track track = sequence[i];

                foreach (var midiEvent in track.Iterator())
                {

                    IMidiMessage midiMessage = midiEvent.MidiMessage;
                    // TODO: Split this switch statements and create separate logic.
                    // We want to split this so that we can expand our functionality later with new keywords for example.
                    // Hint: Command pattern? Strategies? Factory method?

                    if (midiMessage.MessageType == MessageType.Meta)
                    {
                        System.Diagnostics.Debug.WriteLine("Meta: " + (midiEvent.MidiMessage as MetaMessage).MetaType);
                        stringList.Add(getMetaString(midiMessage as MetaMessage, midiEvent, ref lilypondContent));
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Channel: " + (midiEvent.MidiMessage as ChannelMessage).Message);
                        stringList.Add(getChannelString(midiMessage as ChannelMessage, midiEvent, ref lilypondContent));
                    }
                }
            }

            foreach (string s in stringList)
            {
                // Console.WriteLine(s);
            }
            lilypondContent.Append("}");

            return lilypondContent.ToString();

        }

        private string getChannelString(ChannelMessage channelMessage, MidiEvent midiEvent, ref StringBuilder lilyString)
        {
            string returnString = "";
            if (channelMessage.Command == ChannelCommand.NoteOn)
            {
                if (channelMessage.Data2 > 0) // Data2 = loudness
                {
                    // Append the new note.
                    lilyString.Append(MidiToLilyHelper.GetLilyNoteName(_previousMidiKey, channelMessage.Data1));
                    returnString += MidiToLilyHelper.GetLilyNoteName(_previousMidiKey, channelMessage.Data1);
                    _previousMidiKey = channelMessage.Data1;
                    _startedNoteIsClosed = false;
                }
                else if (!_startedNoteIsClosed)
                {
                    // Finish the previous note with the length.
                    double percentageOfBar;
                    string noteLength = MidiToLilyHelper.GetLilypondNoteLength(_previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, _division, _beatNote, _beatsPerBar, out percentageOfBar);
                    lilyString.Append(noteLength);
                    _previousNoteAbsoluteTicks = midiEvent.AbsoluteTicks;
                    lilyString.Append(" ");

                    returnString += noteLength;
                    returnString += " ";

                    _percentageOfBarReached += percentageOfBar;
                    if (_percentageOfBarReached >= 1)
                    {
                        lilyString.AppendLine("|");
                        returnString += "|";
                        _percentageOfBarReached -= 1;
                    }
                    _startedNoteIsClosed = true;
                }
                else
                {
                    returnString += "r";
                    lilyString.Append("r");
                }
            }
            return returnString;
        }
        private string getMetaString(MetaMessage metaMessage, MidiEvent midiEvent, ref StringBuilder lilyString)
        {
            switch (metaMessage.MetaType)
            {
                case MetaType.TimeSignature:
                    return getTimeSignatureString(metaMessage, ref lilyString);
                case MetaType.Tempo:
                    return getTempoString(metaMessage, ref lilyString);
                case MetaType.EndOfTrack:
                    return getEndOfTrackString(metaMessage, midiEvent, ref lilyString);
                default: return "";
            }
        }

        private string getTimeSignatureString(MetaMessage metaMessage, ref StringBuilder lilyString)
        {
            byte[] timeSignatureBytes = metaMessage.GetBytes();
            _beatNote = timeSignatureBytes[0];
            _beatsPerBar = (int)(1 / Math.Pow(timeSignatureBytes[1], -2));
            string returnString = $"\\time {_beatNote}/{_beatsPerBar}";
            lilyString.AppendLine(returnString);
            return returnString;
        }

        private string getTempoString(MetaMessage metaMessage, ref StringBuilder lilyString)
        {
            byte[] tempoBytes = metaMessage.GetBytes();
            int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
            _bpm = 60000000 / tempo;
            string returnString = $"\\tempo 4={_bpm}";
            lilyString.AppendLine(returnString);
            return returnString;
        }

        private string getEndOfTrackString(MetaMessage metaMessage, MidiEvent midiEvent, ref StringBuilder lilyString)
        {
            string returnString = "";
            if (_previousNoteAbsoluteTicks > 0)
            {
                // Finish the last notelength.
                double percentageOfBar;
                string noteLength = MidiToLilyHelper.GetLilypondNoteLength(_previousNoteAbsoluteTicks, midiEvent.AbsoluteTicks, _division, _beatNote, _beatsPerBar, out percentageOfBar);
                returnString += noteLength;
                returnString += " ";
                lilyString.Append(noteLength);
                lilyString.Append(" ");

                _percentageOfBarReached += percentageOfBar;
                if (_percentageOfBarReached >= 1)
                {
                    returnString += "|";
                    lilyString.AppendLine("|");
                    percentageOfBar = percentageOfBar - 1;
                }
            }
            return returnString;
        }
    }
}

