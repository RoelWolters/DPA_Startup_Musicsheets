using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DPA_Musicsheets.Managers
{
	public class MidiSaver : ISaver
	{
		public void save(string fileName, string lilypondText)
		{
			// This method should be functional, not the other method, and the internal helper method should be unnecessary!
			// TODO: Take only this LilypondText, convert it to Song, convert THAT to Midi, and save the resulting Sequence.
			MessageBox.Show("We're going to need some more converters.");
		}

		// For reference purposes. We can't actually access our WPF Staffs or that other information from LilypondViewModel.
		public void save(string fileName, List<MusicalSymbol> WPFStaffs, int bpm, int beatNote, int beatsPerBar)
		{

			Sequence sequence = GetSequenceFromWPFStaffs(WPFStaffs, bpm, beatNote, beatsPerBar);

			sequence.Save(fileName);
		}

		private Sequence GetSequenceFromWPFStaffs(List<MusicalSymbol> WPFStaffs, int bpm, int beatNote, int beatsPerBar)
		{
			List<string> notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
			int absoluteTicks = 0;

			Sequence sequence = new Sequence();

			Track metaTrack = new Track();
			sequence.Add(metaTrack);

			// Calculate tempo
			int speed = 60000000 / bpm;
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

						double relationToQuartNote = beatNote / 4.0;
						double percentageOfBeatNote = 1.0 / beatNote / absoluteLength;
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
						timeSignature[0] = (byte)beatsPerBar;
						timeSignature[1] = (byte)(Math.Log(beatNote) / Math.Log(2));
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
