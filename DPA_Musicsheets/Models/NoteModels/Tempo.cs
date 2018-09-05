using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models {
    class Tempo {
        // Create from correct notation
        public Tempo(int amount, int realScale, int speed) {
            this.amount = amount;
            this.scale = 1 / realScale;
            this.speed = speed;
        }

        // Create from internal notation
        public Tempo(int amount, double internalScale, int speed) {
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
