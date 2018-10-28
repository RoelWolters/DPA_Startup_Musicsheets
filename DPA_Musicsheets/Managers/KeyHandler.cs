using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPA_Musicsheets.Managers
{
	public class KeyHandler {
		public static TextBox textBox = null;
		public static TextBox openFileBox = null;
		private ChainOfResponsibility actionChain = new ChainOfResponsibility();
		private CommandHandler commandHandler = new CommandHandler();
		private List<Key> keysDown = new List<Key>();

		public void KeyDown(KeyEventArgs e) {
			if (keysDown.FindAll(key => key.ToString() == e.Key.ToString()).Count == 0) {
				Console.WriteLine($"Key down: {e.Key}");
				keysDown.Add(e.Key);
				string code = actionChain.Handle(keysDown);
				if (code != null) {
					Console.WriteLine($"Command code activated: {code}");
					if (code == "open") {
						commandHandler.handleCommand(code, openFileBox);
					} else {
						commandHandler.handleCommand(code, textBox);
					}
					
					e.Handled = true;
				}
			}
			
		}

		public void KeyUp(KeyEventArgs e) {
			Console.WriteLine($"Key up: {e.Key}");
			keysDown.Remove(e.Key);
		}

		public void LostFocus() {
			Console.WriteLine("Lost focus");
			keysDown.Clear();
		}

	}
}
