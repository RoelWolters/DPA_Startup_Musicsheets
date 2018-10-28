using DPA_Musicsheets.Managers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.Managers
{
	class CommandHandler {

		// CommandHandler's undo/redo uses a seperate stack from LilypondTextCaretaker's undo/redo.
		// Having our Command and Memento patterns join forces would probably be possible,
		// but that might be a problem too complex to solve in the time we have, and it could violate both patterns.
		// Instead, I decided to focus on learning both patterns individually.
		// When used in isolation, undo/redo should work in both patterns. But never cross the streams!
		public Dictionary<string, Command> commands;
		public List<Command> stack;
		public int stackIndex; // If this is -1, the stack is empty or we are at its beginning (no undo possible).

		public CommandHandler() {
			commands = new Dictionary<string, Command>();
			commands.Add("savelily", new SaveCommand("D:/quicksave.ly"));
			commands.Add("savepdf", new SaveCommand("D:/quicksave.pdf"));
			commands.Add("open", new OpenCommand());
			commands.Add("treble", new InsertTextCommand("\\clef treble"));
			commands.Add("tempo120", new InsertTextCommand("\\tempo 4=120"));
			commands.Add("time4", new InsertTextCommand("\\time 4/4"));
			commands.Add("time3", new InsertTextCommand("\\time 3/4"));
			commands.Add("time6", new InsertTextCommand("\\time 6/8"));
			commands.Add("repeat", new SurroundWithTextCommand("\\repeat volta 2 {\n", "\n}"));
			commands.Add("alternative", new InsertTextCommand("\\alternative { { } { } }"));

			stack = new List<Command>();
			stackIndex = -1;
		}

		public void handleCommand(string key, TextBox textBox) {
			// The command codes "undo" and "redo" are not in our Command Dictionary,
			// but are reserved for the two respective operations on our history stack.
			if (key == "undo") {
				undo();
			} else if (key == "redo") {
				redo();
			} else if (commands.ContainsKey(key)) {
				Command cmdInstance = commands[key].clone();
				cmdInstance.execute(textBox);
				// It doesn't always make sense to have undo/redo functionality. For instance, when opening or saving a file.
				if (cmdInstance.historyStackable) {
					stack = stack.GetRange(0, stackIndex + 1);
					stackIndex++;
					stack.Insert(stackIndex, cmdInstance);
				}
			}
		}

		private void undo() {
			if (stackIndex >= 0) {
				stack[stackIndex].undo();
				stackIndex--;
			}
		}

		private void redo() {
			if (stackIndex < stack.Count-1) {
				stackIndex++;
				stack[stackIndex].redo();
			}
		}
	}
}
