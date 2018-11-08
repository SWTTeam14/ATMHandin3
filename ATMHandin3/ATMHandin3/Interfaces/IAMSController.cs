using System;
using ATMHandin3.Events;

namespace ATMHandin3.Interfaces
{
    public interface IAMSController
    {
        event EventHandler<AircraftsFilteredEventArgs> FilteredAircraftsEvent;
        event EventHandler<TrackEnteredAirspaceEventArgs> TrackEnteredAirspaceEvent;
        event EventHandler<TrackLeftAirspaceEventArgs> TrackLeftAirspaceEvent;
    }
}
