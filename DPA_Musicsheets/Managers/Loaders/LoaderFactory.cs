using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
	public class LoaderFactory {
		public readonly List<string> supportedTypes = new List<string>() { ".ly", ".mid" };

		public ILoader createLoader(string extension)
		{
			if (extension == supportedTypes[0]) {
				return new LilypondLoader();
			} else if (extension == supportedTypes[1]) {
				return new MidiLoader();
			} else {
				return null;
			}
		}
	}
}
