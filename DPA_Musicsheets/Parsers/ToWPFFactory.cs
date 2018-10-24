using DPA_Musicsheets.Models;
using DPA_Musicsheets.Models.NoteModels;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Parsers
{
	class ToWPFFactory {

		public IEnumerable<MusicalSymbol> parseToWPF(Song song, int repeatNum = 0)
		{
			List<MusicalSymbol> symbols = new List<MusicalSymbol>();
			foreach (SongComponent component in song.components)
			{
				switch (component.GetType().Name.ToString())
				{
					case "RepeatComposite":
						symbols.AddRange(parseRepeat((RepeatComposite)component));
						break;
					case "NoteLeaf":
						symbols.Add(parseNote((NoteLeaf)component));
						break;
					case "BarlineLeaf":
						symbols.Add(parseBarline(repeatNum));
						break;
					case "ClefLeaf":
						symbols.Add(parseClef((ClefLeaf)component));
						break;
					case "TempoLeaf":
						symbols.Add(parseTempo((TempoLeaf)component));
						break;
					default:
						Console.WriteLine("Unrecognized SongComponent, skipping.");
						break;
				}
			}
			return symbols;
		}

		private MusicalSymbol parseTempo(TempoLeaf tempo)
		{
			return new TimeSignature(TimeSignatureType.Numbers, (uint)tempo.amount, (uint)tempo.scale);
		}

		private MusicalSymbol parseNote(NoteLeaf note) {
			int modifier = 0;
			MusicalSymbol WPFnote;
			if (note.tone != null)
			{
				if (note.tone.modifier == Modifier.sharp)
				{
					modifier = 1;
				}
				else if (note.tone.modifier == Modifier.b)
				{
					modifier = -1;
				}
				WPFnote = new Note(note.tone.height.ToString(), modifier, note.tone.octave, (MusicalSymbolDuration)(1 / note.length.value), NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single }) { NumberOfDots = note.length.points };
			}
			else
			{
				WPFnote = new Rest((MusicalSymbolDuration)(1 / note.length.value));
			}
			return WPFnote;
		}

		private MusicalSymbol parseBarline(int repeatNum) {
			return new Barline() { AlternateRepeatGroup = repeatNum };
		}

		private MusicalSymbol parseClef(ClefLeaf clef) {
			Clef WPFclef;
			if (clef.type == "bass")
			{
				WPFclef = new Clef(ClefType.FClef, 4);
			}
			else if (clef.type == "treble")
			{
				WPFclef = new Clef(ClefType.GClef, 2);
			}
			else
			{
				return null;
			}
			return WPFclef;
		}

		private IEnumerable<MusicalSymbol> parseRepeat(RepeatComposite repeat) {
			List<MusicalSymbol> symbols = new List<MusicalSymbol>();
			symbols.Add(new Barline() { RepeatSign = RepeatSignType.Forward });
			symbols.AddRange(parseToWPF(new Song() { components = repeat.components }));
			if (repeat.firstAlternative.Count > 0 && repeat.lastAlternative.Count > 0)
			{
				symbols.Add(new Barline() { AlternateRepeatGroup = 1 });
				symbols.AddRange(parseToWPF(new Song() { components = repeat.firstAlternative }, 1));
				symbols.Add(new Barline() { AlternateRepeatGroup = 2 });
				symbols.AddRange(parseToWPF(new Song() { components = repeat.lastAlternative }, 2));
			}
			symbols.Add(new Barline() { RepeatSign = RepeatSignType.Backward });
			return symbols;
		}
	}
}
