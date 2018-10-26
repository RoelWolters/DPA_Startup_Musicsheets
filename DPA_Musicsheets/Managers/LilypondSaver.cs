using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
	public class LilypondSaver : ISaver {
		public void save(string fileName, string LpText) {
			using (StreamWriter outputFile = new StreamWriter(fileName)) {
				outputFile.Write(LpText);
				outputFile.Close();
			}
		}
	}
}
