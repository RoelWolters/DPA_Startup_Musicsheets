using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
    class Song {
        public Song(Tempo tempo, Bar[] melody) {
            this.tempo = tempo;
            this.melody = melody;
        }

        Tempo tempo;
        Bar[] melody;
    }
}
