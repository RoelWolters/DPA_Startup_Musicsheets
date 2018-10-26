using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
	public class SaverFactory
	{
		public readonly List<string> supportedTypes = new List<string>() { ".ly", ".pdf", ".mid" };

		public ISaver createSaver(string extension) {
			if (extension == supportedTypes[0]) {
				return new LilypondSaver();
			} else if (extension == supportedTypes[1]) {
				return new PDFSaver();
			} else if (extension == supportedTypes[2]) {
				return new MidiSaver();
			} else {
				return null;
			}
		}
	}
}
