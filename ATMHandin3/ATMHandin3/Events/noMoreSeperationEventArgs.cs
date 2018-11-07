using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;

namespace ATMHandin3.Events
{
    public class noMoreSeperationEventArgs
    {
        public noMoreSeperationEventArgs(Aircraft aircraft1, Aircraft aircraft2)
        {
            a1 = aircraft1;
            a2 = aircraft2;
        }
        //public List<Aircraft> aircrafts { get; }

        public Aircraft a1 { get; set; }
        public Aircraft a2 { get; set; }
    }
}
