using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Managers
{
	public class PDFSaver : ISaver
	{
		// For the editor, we need to keep track of our LilypondText constantly. No conversion is needed,
		// all we have to do is pass that LilypondText to this function in a nice way.
		public void save(string fileName, string LpText) {
			string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
			string tmpFileName = $"{fileName}-tmp.ly";
			// While we would ordinarily use our domain classes to avoid multiple dependencies, the program Lilypond
			// makes saving to PDF possible, and it needs a .ly file to make a .pdf. We don't actually convert
			// our text to PDF ourselves, and we know nothing about that.
			new LilypondSaver().save(tmpFileName, LpText);

			string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
			string sourceFolder = Path.GetDirectoryName(tmpFileName);
			string sourceFileName = Path.GetFileNameWithoutExtension(tmpFileName);
			string targetFolder = Path.GetDirectoryName(fileName);
			string targetFileName = Path.GetFileNameWithoutExtension(fileName);

			var process = new Process
			{
				StartInfo =
				{
					WorkingDirectory = sourceFolder,
					WindowStyle = ProcessWindowStyle.Hidden,
					Arguments = String.Format("--pdf \"{0}\\{1}.ly\"", sourceFolder, sourceFileName),
					FileName = lilypondLocation
				}
			};

			process.Start();
			while (!process.HasExited)
			{ /* Wait for exit */
			}
			if (sourceFolder != targetFolder || sourceFileName != targetFileName)
			{
				File.Move(sourceFolder + "\\" + sourceFileName + ".pdf", targetFolder + "\\" + targetFileName + ".pdf");
				File.Delete(tmpFileName);
			}
		}
	}
}
