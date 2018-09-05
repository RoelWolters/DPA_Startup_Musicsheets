using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
    public class Tone {
        public Tone(ToneHeight height, Modifier modifier, int octave) {
            this.height = height;
            this.modifier = modifier;
            this.octave = octave;
        }

        ToneHeight height;
        Modifier modifier;
        int octave;
    }
}
