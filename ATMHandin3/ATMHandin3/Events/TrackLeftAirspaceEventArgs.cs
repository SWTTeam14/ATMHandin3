using System;
using ATMHandin3.Classes;

namespace ATMHandin3.Events
{
    public class TrackLeftAirspaceEventArgs : EventArgs
    {
        public TrackLeftAirspaceEventArgs(Aircraft _aircraft)
        {
            aircraft = _aircraft;
        }
        public Aircraft aircraft { get; }
    }
}
