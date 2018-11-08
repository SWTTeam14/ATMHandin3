using System;
using System.Collections.Generic;
using ATMHandin3.Classes;

namespace ATMHandin3.Events
{
    public class AircraftsFilteredEventArgs : EventArgs
    {
        public AircraftsFilteredEventArgs(IDictionary<string, Aircraft> aircrafts)
        {
            filteredAircraft = aircrafts;
        }
        public IDictionary<string, Aircraft> filteredAircraft;
    }
}
