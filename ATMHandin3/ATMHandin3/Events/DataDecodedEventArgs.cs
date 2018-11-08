using System;
using System.Collections.Generic;
using ATMHandin3.Classes;

namespace ATMHandin3.Events
{
    public class DataDecodedEventArgs : EventArgs
    {
        public DataDecodedEventArgs(List<Aircraft> aircrafts)
        {
            Aircrafts = aircrafts;
        }

        public List<Aircraft> Aircrafts { get; }
    }
}
