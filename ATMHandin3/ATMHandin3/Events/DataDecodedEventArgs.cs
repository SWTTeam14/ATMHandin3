using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;

namespace ATMHandin3.Events
{
    public class DataDecodedEventArgs : EventArgs
    {
        public DataDecodedEventArgs(List<Aircraft> aircrafts)
        {
            Aircraft = aircrafts;
        }

        public List<Aircraft> Aircraft { get; }
    }
}
