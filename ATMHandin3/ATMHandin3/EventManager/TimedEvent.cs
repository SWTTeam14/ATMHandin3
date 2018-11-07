using System;
using ATMHandin3.Classes;
using Timer = System.Timers.Timer;

namespace ATMHandin3.EventManager
{
    public abstract class TimedEvent : IEvent
    {
        protected Timer Timer;
        public TimedEvent(Aircraft aircraft)
        {
            Aircraft = aircraft;
            TimeStamp = aircraft.TimeStamp;
            Timer = new System.Timers.Timer();
            Timer.Elapsed += (sender, args) => { Expired?.Invoke(this, System.EventArgs.Empty); };
            Timer.Interval = 5000; // 5 second intervals
            Timer.AutoReset = false; // Repeatable timer
            Timer.Enabled = true;
        }
        public Aircraft Aircraft { get; set; }

        public abstract string Rendition(); 
        public DateTime TimeStamp { get; set; }
        public event EventHandler Expired;
       
    }
}
