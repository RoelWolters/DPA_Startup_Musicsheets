using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
     public class NoteLength {
        public NoteLength(double value, bool point) {
            this.value = value;
            this.point = point;
        }

        double Length {
            get {
                if (point) {
                    return value * 1.5;
                }
                return value;
            }
        }
        double value;
        bool point;
    }
}
