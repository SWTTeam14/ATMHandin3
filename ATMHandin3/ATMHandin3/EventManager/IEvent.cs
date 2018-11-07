using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;

namespace ATMHandin3.EventManager
{
    public interface IEvent
    {
        DateTime TimeStamp { get; set; }
        string Rendition();
        event EventHandler Expired;
    }
}
