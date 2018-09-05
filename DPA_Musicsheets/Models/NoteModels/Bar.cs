using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
    public class Bar {
        public Bar(double length, ModelNote[] notes) {
            this.length = length;
            this.notes = notes;
        }

        ModelNote[] notes;
        double length;
    }
}
