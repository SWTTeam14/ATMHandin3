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
        public event EventHandler<TrackEnteredAirspaceEventArgs> TrackEnteredAirspaceEvent;
        public event EventHandler<TrackLeftAirspaceEventArgs> TrackLeftAirspaceEvent;

        private int eventcounter = 0;

        public IDictionary<string, Aircraft> filteredAircrafts = new Dictionary<string, Aircraft>();

        private IDecoder _decoder;
        private IAirspace _airspace;
        public AMSController(IDecoder decoder, IAirspace airspace)
        {
            _decoder = decoder;
            _airspace = airspace;
            _decoder.DataDecodedEvent += DataDecodedEventHandler;
        }

        public void DataDecodedEventHandler(object sender, DataDecodedEventArgs e)
        {
            foreach (var aircraft in e.Aircrafts)
            {
                if (IsAircraftInside(aircraft, _airspace))
                {
                    if (filteredAircrafts.ContainsKey(aircraft.Tag))
                    {
                        Calculate.Update(filteredAircrafts[aircraft.Tag], aircraft);
                    }

                    if (!filteredAircrafts.ContainsKey(aircraft.Tag))
                    {
                        eventcounter++;
                        Console.WriteLine("EVENT COUNTER: " + eventcounter);
                        TrackEnteredAirspaceEvent(this, new TrackEnteredAirspaceEventArgs(aircraft));
                    }
                    filteredAircrafts[aircraft.Tag] = aircraft;
                }
                else
                {
                    if (filteredAircrafts.ContainsKey(aircraft.Tag))
                    {
                        filteredAircrafts.Remove(aircraft.Tag);

                        TrackLeftAirspaceEvent(this, new TrackLeftAirspaceEventArgs(aircraft));
                    }
                }
            }
            FilteredAircraftsEvent?.Invoke(this, new AircraftsFilteredEventArgs(filteredAircrafts));

            //onFilteredAircraftsEvent(new AircraftsFilteredEventArgs(filteredAircrafts));
        }

        public bool IsAircraftInside(Aircraft aircraft, IAirspace airspace)
        {
            return aircraft.XCoordinate <= airspace.East && aircraft.XCoordinate >= airspace.West && aircraft.YCoordinate <= airspace.North && aircraft.YCoordinate >= airspace.South &&
                   aircraft.Altitude >= airspace.LowerAltitude && aircraft.Altitude <= airspace.UpperAltitude;
        }

        public void Print()
        {
            foreach (var entry in filteredAircrafts.ToList().OrderBy(x => x.Key))
            {
                Console.WriteLine(entry.Value.ToString()); ;
            }
        }
    }
}
