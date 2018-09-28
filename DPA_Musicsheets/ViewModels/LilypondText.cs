using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.ViewModels {
	class LilypondText {

		public String Text { get; set; }
		
		public LilypondTextMemento saveMemento() {
			return new LilypondTextMemento(Text);
		}

		public void restoreMemento(LilypondTextMemento memento) {
			Text = memento.Text;
		}

	}
}
