using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;

namespace ATMHandin3.Events
{
    public class AircraftsFilteredEventArgs
    {
        public AircraftsFilteredEventArgs(List<Aircraft> aircrafts)
        {
            filteredAircraft = aircrafts;
        }
        public List<Aircraft> filteredAircraft;
    }
}
