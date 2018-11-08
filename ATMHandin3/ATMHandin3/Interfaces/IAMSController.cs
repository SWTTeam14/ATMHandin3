using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;
using ATMHandin3.Events;

namespace ATMHandin3.Interfaces
{
    public interface IAMSController
    {
        event EventHandler<AircraftsFilteredEventArgs> FilteredAircraftsEvent;
        event EventHandler<TrackEnteredAirspaceEventArgs> TrackEnteredAirspaceEvent;
        event EventHandler<TrackLeftAirspaceEventArgs> TrackLeftAirspaceEvent;

        bool IsAircraftInside(Aircraft aircraft, IAirspace airspace);
    }
}
