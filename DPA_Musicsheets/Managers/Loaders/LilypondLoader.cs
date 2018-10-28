using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
	public class LilypondLoader : ILoader { 
		public string load(string fileName) {
			StringBuilder sb = new StringBuilder();
			foreach (var line in File.ReadAllLines(fileName))
			{
				sb.AppendLine(line);
			}

			return sb.ToString();
		}
	}
}
