using System;
using System.Collections.Generic;
using ATMHandin3.Classes;
using ATMHandin3.Events;

namespace ATMHandin3.Interfaces
{
    public interface ICollisionAvoidanceSystem
    {
        event EventHandler<SeparationEventArgs> SeparationEvent;
        event EventHandler<SeparationEventArgs> SeparationAvoidedEvent;
        void CollisionWarning(IDictionary<string, Aircraft> aircrafts);
    }
}
