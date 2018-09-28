using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.ViewModels {
	class LilypondTextMemento {

		public string Text { get; }

		public LilypondTextMemento(string text) {
			Text = text;
		}
	}
}
