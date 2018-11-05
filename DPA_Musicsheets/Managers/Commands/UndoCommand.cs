using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.Managers.Commands
{
	class UndoCommand : Command {
		public override void execute(ref int stackIndex, ref List<Command> stack) {
			if (stackIndex >= 0) {
				stack[stackIndex].undo();
				stackIndex--;
			}
		}

		public override Command clone() {
			UndoCommand newCommand = new UndoCommand();
			newCommand.textBox = textBox;
			newCommand.originalText = originalText;
			newCommand.newText = newText;
			return newCommand;
		}
	}
}
