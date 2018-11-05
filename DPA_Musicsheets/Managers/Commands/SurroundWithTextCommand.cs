using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.Managers.Commands
{
	class SurroundWithTextCommand : Command {
		private string textBefore;
		private string textAfter;

		public SurroundWithTextCommand(string textBefore, string textAfter) {
			this.textBefore = textBefore;
			this.textAfter = textAfter;
		}

		public override void execute(ref int stackIndex, ref List<Command> stack) {
			if (textBox != null) {
				originalText = textBox.Text;
				string originalSelection = textBox.SelectedText;
				if (textBox.IsKeyboardFocused){
					textBox.SelectedText = textBefore + originalSelection + textAfter;
				}
				newText = textBox.Text;

				stack = stack.GetRange(0, stackIndex + 1);
				stackIndex++;
				stack.Insert(stackIndex, this);
			}
		}

		public override Command clone() {
			SurroundWithTextCommand newCommand = new SurroundWithTextCommand(textBefore, textAfter);
			newCommand.textBox = textBox;
			newCommand.originalText = originalText;
			newCommand.newText = newText;
			return newCommand;
		}
	}
}
