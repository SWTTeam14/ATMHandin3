using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;

namespace ATMHandin3.EventManager
{
    public class TrackLeftAirspaceEvent : TimedEvent
    {
        public TrackLeftAirspaceEvent(Aircraft aircraft) : base(aircraft)
        {
        }
        public override string Rendition()
        {
            string dateTimeString = TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss fff");
            return string.Format("\nAIRCRAFT LEFT AIRSPACE EVENT: Aircraft with tag:{0} just left the airspace at time:{1}", Aircraft.Tag, dateTimeString);
        }
    }
}

