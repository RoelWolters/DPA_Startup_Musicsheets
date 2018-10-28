using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DPA_Musicsheets.Managers.Commands
{
	class SaveCommand : Command {
		private ISaver saver;
		private string filename;

		public SaveCommand(string filename) {
			// This functionality doesn't make any alterations within the program, so there wouldn't be anything to undo or redo.
			historyStackable = false;
			this.filename = filename;
			saver = new SaverFactory().createSaver("." + filename.Split('.').Last());
		}

		public override void execute(TextBox textBox) {
			if (saver != null) {
				saver.save(filename, textBox.Text);
				MessageBox.Show($"Saved as {filename} successfully.");
			} else {
				MessageBox.Show($"Could not quick save file.");
			}
		}

		public override Command clone() {
			SaveCommand newCommand = new SaveCommand(filename);
			newCommand.textBox = textBox;
			newCommand.originalText = originalText;
			newCommand.newText = newText;
			return newCommand;
		}
	}
}
