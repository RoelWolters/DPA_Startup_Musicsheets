using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers.Openers
{
	public static class ChooseFile {

		public static string getFileChoice() {
			OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi or LilyPond files (*.mid *.ly)|*.mid;*.ly" };
			if (openFileDialog.ShowDialog() == true) {
				return openFileDialog.FileName;
			} else {
				return null;
			}
		}

	}
}
