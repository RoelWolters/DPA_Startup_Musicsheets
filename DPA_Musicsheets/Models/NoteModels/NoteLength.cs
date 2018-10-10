using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
     public class NoteLength {
        public NoteLength(double value, int points) {
            this.value = value;
            this.points = points;
        }

        double Length {
            get {
				double returnValue = value;
                for (int i=1;i<points;i++) {
					returnValue += 0.5 * value;
                }
                return returnValue;
            }
        }
        double value;
        int points;
    }
}
