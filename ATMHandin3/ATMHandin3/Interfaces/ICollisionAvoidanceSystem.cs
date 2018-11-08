using System;
using ATMHandin3.Events;

namespace ATMHandin3.Interfaces
{
    public interface ICollisionAvoidanceSystem
    {
        event EventHandler<SeparationEventArgs> SeparationEvent;
    }
}
