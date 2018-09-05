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
            this.tempoChange = null;
        }

        public ModelNote(Tone tone, NoteLength length, Tempo tempoChange) {
            this.tone = tone;
            this.length = length;
            this.tempoChange = tempoChange;
        }

        Tempo tempoChange;
        Tone tone;
        NoteLength length;
    }
}
