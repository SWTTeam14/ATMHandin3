using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMHandin3.EventManager
{
    public class EventManager
    {
        public List<IEvent> EventList { get; }

        public EventManager()
        {
            EventList = new List<IEvent>();
        }
        
        public void AddEvent(IEvent e)
        {
            EventList.Add(e);
            e.Expired += (sender, args) => { EventList.Remove(e);};
        }

        public void RemoveEvent(IEvent e)
        {
            EventList.Remove(e);
        }
    }
}
