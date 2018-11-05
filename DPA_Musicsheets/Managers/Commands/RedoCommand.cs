using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.Managers.Commands
{
	class RedoCommand : Command {
		public override void execute(ref int stackIndex, ref List<Command> stack) {
			if (stackIndex < stack.Count - 1) {
				stackIndex++;
				stack[stackIndex].redo();
			}
		}

		public override Command clone() {
			RedoCommand newCommand = new RedoCommand();
			newCommand.textBox = textBox;
			newCommand.originalText = originalText;
			newCommand.newText = newText;
			return newCommand;
		}
	}
}
