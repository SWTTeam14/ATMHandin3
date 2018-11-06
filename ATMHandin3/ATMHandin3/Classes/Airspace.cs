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
        public Airspace()
        {
            South = 10000;
            West = 10000;
            North = 90000;
            East = 90000;
            LowerAltitude = 500;
            UpperAltitude = 20000;

        }
        public int South { get; set; }
        public int West { get; set; }
        public int North { get; set; }
        public int East { get; set; }
        public int LowerAltitude { get; set; }
        public int UpperAltitude { get; set; }


    }
}
