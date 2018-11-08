using System;
using System.Collections.Generic;
using ATMHandin3.Classes;

namespace ATMHandin3.Events
{
    public class OnTimerEventArgs : EventArgs
    {
        public OnTimerEventArgs(IDictionary<string, Aircraft> aircrafts)
        {
            filteredAircraft = aircrafts;
        }
        public IDictionary<string, Aircraft> filteredAircraft;
    }
}
