using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class Airspace : IAirspace
    {
        public Airspace(int South, int West, int North, int East, int LowerAltitude, int UpperAltitude)
        {
            this.South = South;
            this.West = West;
            this.North = North;
            this.East = East;
            this.LowerAltitude = LowerAltitude;
            this.UpperAltitude = UpperAltitude;
            
        }
        public int South { get; set; }
        public int West { get; set; }
        public int North { get; set; }
        public int East { get; set; }
        public int LowerAltitude { get; set; }
        public int UpperAltitude { get; set; }


    }
}
