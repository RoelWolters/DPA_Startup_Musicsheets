using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
    public class TempoLeaf : SongLeaf {
        // Create from correct notation
        public TempoLeaf(int amount, int realScale, int speed) {
            this.amount = amount;
            this.scale = 1 / realScale;
            this.speed = speed;
        }

        // Create from internal notation
        public TempoLeaf(int amount, double internalScale, int speed) {
            this.amount = amount;
            this.scale = internalScale;
            this.speed = speed;
        }

		// Calculate correct notation
		double RealScale { get {
                return 1 / scale;
            }
        }
        int amount;
        double scale;
        int speed;
    }
}
