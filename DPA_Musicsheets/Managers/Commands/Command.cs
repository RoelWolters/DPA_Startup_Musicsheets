using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets.Managers
{
	abstract class Command {
		protected string originalText; // The state the textBox was in right before executing our Command, undo() to this.
		protected string newText; // The state the textBox is in right after executing our Command, redo() to this.
		public TextBox textBox = null; // The TextBox this Command should manipulate.

		public abstract void execute(ref int stackIndex, ref List<Command> stack);

		public virtual void undo() {
			if (originalText != null && textBox != null) {
				textBox.Text = originalText;
			}
		}

		public virtual void redo() {
			if (newText != null && textBox != null) {
				textBox.Text = newText;
			}
		}

		// Our abstract Command is also an abstract Prototype.
		// After all, we want to keep a stack of used Commands in our CommandHandler,
		// but keep the Commands in our Dictionary fresh and unused.
		// What if we want to use the same Command twice in a row, and then undo both of them?
		public abstract Command clone();
	}
}
