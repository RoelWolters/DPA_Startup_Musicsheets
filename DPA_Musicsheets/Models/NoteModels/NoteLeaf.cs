using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
    public class NoteLeaf : SongLeaf {
        public NoteLeaf(Tone tone, NoteLength length) {
            this.tone = tone;
            this.length = length;
        }

		public NoteLeaf(NoteLength length) {
			this.tone = null;
			this.length = length;
		}

		public Tone tone; // A null value indicates a rest.
        public NoteLength length;

		
    }
}
