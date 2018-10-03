using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.ViewModels {
	class LilypondTextCaretaker {

		private LilypondText originator;
		private ArrayList actions;
		private int current;

		public LilypondTextCaretaker(LilypondText originator) {
			this.originator = originator;
			reset();
		}

		// A new file has been loaded in the originator
		// or we need to initialize ourselves.
		public void reset() {
			actions = new ArrayList();
			current = -1; // If current is -1, that means there is no 'current' textbox state.
			// We should change() to create an initial state whenever we reset().
		}

		// Text has changed in the originator.
		// Add a new memento. All previously
		// redoable actions are removed.
		public void change() {
			actions = actions.GetRange(0, current+1);
			current++;
			actions.Insert(current, originator.saveMemento());
		}

		// Should our 'undo' button be available?
		public bool canUndo() {
			return current > 0;
		}

		// User selected 'undo'. Move our 'current'
		// pointer one position back if possible
		// and restore that memento to the originator.
		public void undo() {
			if (canUndo()) {
				current--;
				originator.restoreMemento((LilypondTextMemento)actions[current]);
			}
		}

		// Should our 'redo' button be available?
		public bool canRedo() {
			return current != -1 && current < actions.Count - 1;
		}

		// User selected 'redo'. Move our 'current'
		// pointer one position forward if possible
		// and restore that memento to the originator.
		public void redo() {
			if (canRedo()) {
				current++;
				originator.restoreMemento((LilypondTextMemento)actions[current]);
			}
		}

	}
}
