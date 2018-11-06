using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;

namespace ATMHandin3.Events
{
    public class SeparationEventArgs : EventArgs
    {
        public SeparationEventArgs(List<Aircraft> _aircrafts)
        {
            aircrafts = _aircrafts;
        }
        public List<Aircraft> aircrafts { get; }
    }
}
