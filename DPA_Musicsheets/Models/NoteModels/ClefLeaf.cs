using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models.NoteModels
{
	class ClefLeaf : SongLeaf {
		public string type;

		public ClefLeaf(string type) {
			this.type = type;
		}

	}
}
