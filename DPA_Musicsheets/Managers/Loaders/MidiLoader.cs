using DPA_Musicsheets.Factories;
using DPA_Musicsheets.Parsers;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
	public class MidiLoader : ILoader {

		public string load(string fileName) {
			Sequence midiSequence;
			midiSequence = new Sequence();
			midiSequence.Load(fileName);

			ParserFactory factory = new ParserFactory();
			IParser parser = factory.CreateParser(".mid");

			return parser.Parse(midiSequence);
		}

	}
}
