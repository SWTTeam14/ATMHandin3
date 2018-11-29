using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Events;

namespace ATMHandin3.Interfaces
{
    public interface IConsoleOutput
    {
        void CollisionEventHandler(object sender, SeparationEventArgs e);
        void CollisionAvoidedEventHandler(object sender, SeparationEventArgs e);
        void TrackEnteredAirspaceEventHandler(object sender, TrackEnteredAirspaceEventArgs e);
        void TrackLeftAirspaceEventHandler(object sender, TrackLeftAirspaceEventArgs e);
        void AircraftsInsideAirspaceEventHandler(object sender, AircraftsFilteredEventArgs e);
        void OutputAircraftsInsideAirspace(AircraftsFilteredEventArgs e);
        void OutputAircraftsWhoJustEnteredAirspace();
        void OutputAircraftsWhoJustExitedAirspace();
        void OutputAircraftsColliding();
    }
}
