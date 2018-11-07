using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;

namespace ATMHandin3.EventManager
{
    public class SeparationEvent : IEvent
    {
        public SeparationEvent(Aircraft a1, Aircraft a2)
        {
            Aircraft1 = a1;
            Aircraft2 = a2;
            TimeStamp = a1.TimeStamp > a2.TimeStamp ? a1.TimeStamp : a2.TimeStamp;
        }
        public Aircraft Aircraft1 { get; set; }
        public Aircraft Aircraft2 { get; set; }

        public string Rendition()
        {
            string dateTimeString = TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss fff");
            return string.Format("AIRCRAFTS ARE COLLIDING: Aircraft with tag: {0} and {2} are colliding at time: {1}", Aircraft1.Tag, dateTimeString, Aircraft2.Tag);
        }
        public DateTime TimeStamp { get; set; }

        public event EventHandler Expired;
    }
}
