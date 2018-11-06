using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;

namespace ATMHandin3.Events
{
    public class TrackEnteredAirspaceEventArgs : EventArgs
    {
        public TrackEnteredAirspaceEventArgs(Aircraft _aircraft)
        {
            aircraft = _aircraft;
        }
        public Aircraft aircraft { get; }
    }
}
