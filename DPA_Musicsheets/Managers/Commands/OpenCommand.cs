using DPA_Musicsheets.Managers.Openers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DPA_Musicsheets.Managers.Commands
{
	class OpenCommand : Command {

		public OpenCommand() {
			// While we could use our undo and redo functionality perfectly well here and without difficulty, it would not make sense in this context.
			// I made the conscious decision to isolate undo/redo functionality to Commands that manipulate the Lilypond textbox.
			historyStackable = false;
		}

		// The TextBox that should be passed to this method is the one that shows the filename to be opened.
		public override void execute(TextBox textBox) {
			textBox.Text = ChooseFile.getFileChoice();
		}

		public override Command clone() {
			OpenCommand newCommand = new OpenCommand();
			newCommand.textBox = textBox;
			newCommand.originalText = originalText;
			newCommand.newText = newText;
			return newCommand;
		}

	}
}
