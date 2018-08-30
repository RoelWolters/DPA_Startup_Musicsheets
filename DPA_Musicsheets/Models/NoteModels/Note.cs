using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
    class ModelNote {
        public ModelNote(Tone tone, NoteLength length) {
            this.tone = tone;
            this.length = length;
        }

        Tone tone;
        NoteLength length;
    }
}
