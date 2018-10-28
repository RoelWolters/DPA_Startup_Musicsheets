using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.Managers.Commands
{
	class InsertTextCommand : Command {
		private string textToInsert;

		public InsertTextCommand(string text) {
			textToInsert = text;
		}

		public override void execute(TextBox textBox) {
			if (textBox != null) {
				originalText = textBox.Text;
				if (textBox.IsKeyboardFocused) {
					textBox.SelectedText = textBox.SelectedText.Insert(0, textToInsert);
				}
				
				this.textBox = textBox;
				newText = textBox.Text;
			}
		}

		public override Command clone() {
			InsertTextCommand newCommand = new InsertTextCommand(textToInsert);
			newCommand.textBox = textBox;
			newCommand.originalText = originalText;
			newCommand.newText = newText;
			return newCommand;
		}
	}
}
