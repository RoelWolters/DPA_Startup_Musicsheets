using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPA_Musicsheets.Managers
{
	public class ChainElement {
		// The element to delegate to when we don't take responsibility.
		public ChainElement nextElement;

		// The key to return when we take responsibility. Not strictly part of Chain of Responsibility,
		// as normally we just return a bool, but this is what needs to be done if we want our CommandHandler
		// to be the one to actually execute commands. The alternative is storing a 
		// Command in each ChainElement, making ChainElements responsible for calling their commands.
		// But that would mean no centralized command list and the coupling would be tighter!
		public string associatedCode;

		// The key combination that will activate this particular chain element.
		// Originally, I had ChainElement as an abstract class, with a derived class for each key combination.
		// But I feel that this serves the 'hypothetically customizable' mindset better.
		public List<Key> keyCombo;

		public ChainElement(string associatedCode, List<Key> keyCombo) {
			this.associatedCode = associatedCode;
			this.keyCombo = keyCombo;
		}

		public string Handle(List<Key> keysDown) {
			if (TryHandle(keysDown)) {
				return associatedCode; // We can handle this, our command key goes to the top.
			} else if (nextElement != null) {
				return nextElement.Handle(keysDown); // Not the end of the line and we can't handle this, delegate to next element.
			} else {
				return null; // End of the line and we can't handle this, null goes to the top.
			}
		}

		private bool TryHandle(List<Key> keysDown) {
			// Our list versus the entered list: both must have the same elements in the same order.
			return keysDown.SequenceEqual(keyCombo);
		}
	}
}
