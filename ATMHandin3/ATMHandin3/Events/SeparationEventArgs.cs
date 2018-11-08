using System;
using ATMHandin3.Classes;

namespace ATMHandin3.Events
{
    public class SeparationEventArgs : EventArgs
    {
        public SeparationEventArgs(Aircraft aircraft1, Aircraft aircraft2)
        {
            a1 = aircraft1;
            a2 = aircraft2;
        }
        
        public Aircraft a1 { get; set; }
        public Aircraft a2 { get; set; }
    }
}
