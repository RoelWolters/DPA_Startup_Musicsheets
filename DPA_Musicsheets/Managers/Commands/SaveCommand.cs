using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DPA_Musicsheets.Managers.Commands
{
	class SaveCommand : Command {
		private string tempFileName;

		public SaveCommand(string tempFileName) {
			this.tempFileName = tempFileName;
		}

		public override void execute(ref int stackIndex, ref List<Command> stack) {
			SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
			saveFileDialog.FileName = tempFileName;
			if (saveFileDialog.ShowDialog() == true) {
				string extension = Path.GetExtension(saveFileDialog.FileName);
				SaverFactory factory = new SaverFactory();
				ISaver saver = factory.createSaver(extension);
				if (saver != null) {
					saver.save(saveFileDialog.FileName, textBox.Text);
				} else {
					MessageBox.Show($"Extension {extension} is not supported.");
				}
			}
		}

		public override Command clone() {
			SaveCommand newCommand = new SaveCommand(tempFileName);
			newCommand.textBox = textBox;
			newCommand.originalText = originalText;
			newCommand.newText = newText;
			return newCommand;
		}
	}
}
