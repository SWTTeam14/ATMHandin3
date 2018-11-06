using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class AMSController : IAMSController
    {
        public event EventHandler<AircraftsFilteredEventArgs> FilteredAircraftsEvent;
        private List<Aircraft> filteredAircrafts; 

        private IDecoder _decoder;
        private IAirspace _airspace;
        public AMSController(IDecoder decoder, IAirspace airspace)
        {
            //hsadahsd
            _decoder = decoder;
            _airspace = airspace;

            _decoder.DataDecodedEvent += DataDecodedEventHandler;
        }

        public void DataDecodedEventHandler(object sender, DataDecodedEventArgs e)
        {
            foreach (var Aircraft in e.Aircraft)
            {

                if (IsAircraftInside(Aircraft, _airspace))
                {
                    filteredAircrafts.Add(Aircraft);
                        
                }
                else if (!IsAircraftInside(Aircraft, _airspace))
                {
                    filteredAircrafts.Remove(Aircraft);
                }
            }
        }


        public bool IsAircraftInside(Aircraft aircraft, IAirspace airspace)
        {
            return aircraft.XCoordinate <= airspace.East && aircraft.XCoordinate >= airspace.West && aircraft.YCoordinate <= airspace.North && aircraft.YCoordinate >= airspace.South &&
                   aircraft.Altitude >= airspace.LowerAltitude && aircraft.Altitude <= airspace.UpperAltitude;
        }


    }
}
