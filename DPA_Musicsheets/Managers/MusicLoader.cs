
using DPA_Musicsheets.Factories;
using DPA_Musicsheets.Models;
using DPA_Musicsheets.Models.NoteModels;
using DPA_Musicsheets.Parsers;
using DPA_Musicsheets.ViewModels;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
    /// <summary>
    /// This is the one and only god class in the application.
    /// It knows all about all file types, knows every viewmodel and contains all logic.
    /// TODO: Clean this class up.
    /// </summary>
    public class MusicLoader
    {
        #region Properties
        public string LilypondText { get; set; }
        public List<MusicalSymbol> WPFStaffs { get; set; } = new List<MusicalSymbol>();
        private static List<Char> notesorder = new List<Char> { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };

        public Sequence MidiSequence { get; set; }
        #endregion Properties

        private int _beatNote = 4;    // De waarde van een beatnote.
        private int _bpm = 120;       // Aantal beatnotes per minute.
        private int _beatsPerBar;     // Aantal beatnotes per maat.

        public MainViewModel MainViewModel { get; set; }
        public LilypondViewModel LilypondViewModel { get; set; }
        public MidiPlayerViewModel MidiPlayerViewModel { get; set; }
        public StaffsViewModel StaffsViewModel { get; set; }

        /// <summary>
        /// This creates WPF staffs and MIDI from Lilypond.
        /// TODO: Remove the dependencies from one language to another. If we want to replace the WPF library with another for example, we have to rewrite all logic.
        /// TODO: Create our own domain classes to be independent of external libraries/languages.
        /// </summary>
        /// <param name="content"></param>
        public void LoadLilypondIntoWpfStaffsAndMidi(string content)
        {
            LilypondText = content;
            content = content.Trim().ToLower().Replace("\r\n", " ").Replace("\n", " ").Replace("  ", " ");
            LinkedList<LilypondToken> tokens = GetTokensFromLilypond(content);
            WPFStaffs.Clear();

			// We use a hard-coded Song to test Domain to WPF converting, as we don't have a Lilypond to Domain converter.
			WPFStaffs.AddRange(new ToWPFConverter().parseToWPF(new Song() {
				components = new List<SongComponent>() {
					new ClefLeaf("treble"),
					new TempoLeaf(4, 4, 120),
					new NoteLeaf(new Tone(ToneHeight.C, Modifier.b, 4), new NoteLength(1, 0)),
					new NoteLeaf(new Tone(ToneHeight.D, Modifier.none, 4), new NoteLength(0.5, 0)),
					new NoteLeaf(new Tone(ToneHeight.E, Modifier.none, 4), new NoteLength(0.25, 0)),
					new NoteLeaf(null, new NoteLength(0.25, 0)),
					new NoteLeaf(new Tone(ToneHeight.F, Modifier.none, 4), new NoteLength(0.125, 0)),
					new NoteLeaf(new Tone(ToneHeight.G, Modifier.none, 4), new NoteLength(0.0625, 0)),
					new NoteLeaf(new Tone(ToneHeight.A, Modifier.none, 4), new NoteLength(0.0625, 0)),
					new NoteLeaf(new Tone(ToneHeight.B, Modifier.none, 4), new NoteLength(0.03125, 0)),
					new NoteLeaf(new Tone(ToneHeight.C, Modifier.sharp, 5), new NoteLength(0.03125, 0)),
					new RepeatComposite(3) {
						components = new List<SongComponent>() {
							new NoteLeaf(new Tone(ToneHeight.C, Modifier.b, 4), new NoteLength(1, 0)),
							new NoteLeaf(new Tone(ToneHeight.D, Modifier.none, 4), new NoteLength(0.5, 0)),
							new NoteLeaf(new Tone(ToneHeight.E, Modifier.none, 4), new NoteLength(0.25, 0))
						},
						firstAlternative = new List<SongComponent>() {
							new NoteLeaf(new Tone(ToneHeight.F, Modifier.none, 4), new NoteLength(0.125, 0)),
							new NoteLeaf(new Tone(ToneHeight.G, Modifier.none, 4), new NoteLength(0.25, 0)),
							new NoteLeaf(new Tone(ToneHeight.A, Modifier.none, 4), new NoteLength(0.25, 0)),
						},
						lastAlternative = new List<SongComponent>() {
							new NoteLeaf(new Tone(ToneHeight.B, Modifier.none, 4), new NoteLength(0.5, 0)),
							new NoteLeaf(new Tone(ToneHeight.C, Modifier.sharp, 5), new NoteLength(0.5, 0)),
						}
					}
				}
			}));
            //WPFStaffs.AddRange(GetStaffsFromTokens(tokens));
            this.StaffsViewModel.SetStaffs(this.WPFStaffs);

            MidiSequence = GetSequenceFromWPFStaffs();
            MidiPlayerViewModel.MidiSequence = MidiSequence;
        }

        #region Staffs loading (loads lilypond to WPF staffs)

		private static LinkedList<LilypondToken> GetTokensFromLilypond(string content)
        {
            var tokens = new LinkedList<LilypondToken>();

            foreach (string s in content.Split(' ').Where(item => item.Length > 0))
            {
                LilypondToken token = new LilypondToken()
                {
                    Value = s
                };

                switch (s)
                {
                    case "\\relative": token.TokenKind = LilypondTokenKind.Staff; break;
                    case "\\clef": token.TokenKind = LilypondTokenKind.Clef; break;
                    case "\\time": token.TokenKind = LilypondTokenKind.Time; break;
                    case "\\tempo": token.TokenKind = LilypondTokenKind.Tempo; break;
                    case "\\repeat": token.TokenKind = LilypondTokenKind.Repeat; break;
                    case "\\alternative": token.TokenKind = LilypondTokenKind.Alternative; break;
                    case "{": token.TokenKind = LilypondTokenKind.SectionStart; break;
                    case "}": token.TokenKind = LilypondTokenKind.SectionEnd; break;
                    case "|": token.TokenKind = LilypondTokenKind.Bar; break;
                    default: token.TokenKind = LilypondTokenKind.Unknown; break;
                }

                if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"[~]?[a-g][,'eis]*[0-9]+[.]*").IsMatch(s))
                {
                    token.TokenKind = LilypondTokenKind.Note;
                }
                else if (token.TokenKind == LilypondTokenKind.Unknown && new Regex(@"r.*?[0-9][.]*").IsMatch(s))
                {
                    token.TokenKind = LilypondTokenKind.Rest;
                }

                if (tokens.Last != null)
                {
                    tokens.Last.Value.NextToken = token;
                    token.PreviousToken = tokens.Last.Value;
                }

                tokens.AddLast(token);
            }

            return tokens;
        }
		#endregion Staffs loading (loads lilypond to WPF staffs)

		// I created an interface called ISaver. It takes a filename and LilypondText in its save() method. Our MidiSaver derives from it.
		// To do this, go to that class, convert LilypondText to our model Song, convert that to a midi Sequence, and finally save that sequence.
		// We should probably have a class for both of these conversions, too.
        
        /// <summary>
        /// We create MIDI from WPF staffs, 2 different dependencies, not a good practice.
        /// TODO: Create MIDI from our own domain classes.
        /// TODO: Our code doesn't support repeats (rendering notes multiple times) in midi yet. Maybe with a COMPOSITE this will be easier?
        /// </summary>
        /// <returns></returns>
        private Sequence GetSequenceFromWPFStaffs()
        {
            List<string> notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
            int absoluteTicks = 0;

            Sequence sequence = new Sequence();

            Track metaTrack = new Track();
            sequence.Add(metaTrack);

            // Calculate tempo
            int speed = (60000000 / _bpm);
            byte[] tempo = new byte[3];
            tempo[0] = (byte)((speed >> 16) & 0xff);
            tempo[1] = (byte)((speed >> 8) & 0xff);
            tempo[2] = (byte)(speed & 0xff);
            metaTrack.Insert(0 /* Insert at 0 ticks*/, new MetaMessage(MetaType.Tempo, tempo));

            Track notesTrack = new Track();
            sequence.Add(notesTrack);

            for (int i = 0; i < WPFStaffs.Count; i++)
            {
                var musicalSymbol = WPFStaffs[i];
                switch (musicalSymbol.Type)
                {
                    case MusicalSymbolType.Note:
                        Note note = musicalSymbol as Note;

                        // Calculate duration
                        double absoluteLength = 1.0 / (double)note.Duration;
                        absoluteLength += (absoluteLength / 2.0) * note.NumberOfDots;

                        double relationToQuartNote = _beatNote / 4.0;
                        double percentageOfBeatNote = (1.0 / _beatNote) / absoluteLength;
                        double deltaTicks = (sequence.Division / relationToQuartNote) / percentageOfBeatNote;

                        // Calculate height
                        int noteHeight = notesOrderWithCrosses.IndexOf(note.Step.ToLower()) + ((note.Octave + 1) * 12);
                        noteHeight += note.Alter;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 90)); // Data2 = volume

                        absoluteTicks += (int)deltaTicks;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 0)); // Data2 = volume

                        break;
                    case MusicalSymbolType.TimeSignature:
                        byte[] timeSignature = new byte[4];
                        timeSignature[0] = (byte)_beatsPerBar;
                        timeSignature[1] = (byte)(Math.Log(_beatNote) / Math.Log(2));
                        metaTrack.Insert(absoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignature));
                        break;
                    default:
                        break;
                }
            }

            notesTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            metaTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            return sequence;
        }

    }
}
