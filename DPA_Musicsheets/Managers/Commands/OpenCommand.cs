﻿using DPA_Musicsheets.Managers.Openers;
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

		// The TextBox that should be passed to this method is the one that shows the filename to be opened.
		public override void execute(ref int stackIndex, ref List<Command> stack) {
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
