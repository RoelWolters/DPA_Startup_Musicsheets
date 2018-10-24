using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
    public class TempoLeaf : SongLeaf {
        // Create from correct notation
        public TempoLeaf(int amount, int scale, int speed) {
            this.amount = amount;
			this.scale = scale;
            this.speed = speed;
        }

        public int amount;
        public int scale;
        public int speed;
    }
}
