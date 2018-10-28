using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPA_Musicsheets.Managers
{
	class ChainOfResponsibility
	{
		// Could hypothetically be changed at runtime to another first element 
		// (chained together with all next elements like a linked list) at any time.
		public ChainElement first;

		public ChainOfResponsibility() {
			// Alt has been substituted for LeftShift due to Alt being masked as Key.System some of the time.

			// Key combinations that are a subset of bigger combinations were changed, as entering the smaller combo would 
			// result in that combo being accepted before you had time to type the bigger combo you meant to type.

			// The special operations 'undo' and 'redo' use LeftShift, as Ctrl-Z/Ctrl-Y already activate Windows' undo/redo.

			// Shift-T-(number) was changed to Shift-(number), because for some reason extra numbers are not registered after Shift-T.
			List<ChainElement> allElements = new List<ChainElement>() {
				new ChainElement("undo", new List<Key>() {Key.LeftShift, Key.Z}),
				new ChainElement("redo", new List<Key>() {Key.LeftShift, Key.Y}),
				new ChainElement("savelily", new List<Key>() {Key.LeftCtrl, Key.S, Key.L}),
				new ChainElement("savepdf", new List<Key>() {Key.LeftCtrl, Key.S, Key.P}),
				new ChainElement("open", new List<Key>() {Key.LeftCtrl, Key.O}),
				new ChainElement("treble", new List<Key>() {Key.LeftShift, Key.C}),
				new ChainElement("tempo120", new List<Key>() {Key.LeftShift, Key.S}),
				new ChainElement("time4", new List<Key>() {Key.LeftShift, Key.T}),
				new ChainElement("time4", new List<Key>() {Key.LeftShift, Key.D4}),
				new ChainElement("time3", new List<Key>() {Key.LeftShift, Key.D3}),
				new ChainElement("time6", new List<Key>() {Key.LeftShift, Key.D6}),
				new ChainElement("repeat", new List<Key>() {Key.LeftShift, Key.R}),
				new ChainElement("alternative", new List<Key>() {Key.LeftShift, Key.A}),
			};

			for (int i=0;i<allElements.Count-1;i++) {
				if (i == 0) {
					first = allElements[i];
				}
				allElements[i].nextElement = allElements[i + 1];
			}
		}

		public string Handle(List<Key> keysDown) {
			if (first != null) {
				return first.Handle(keysDown);
			} else {
				return null;
			}
		}
	}
}
