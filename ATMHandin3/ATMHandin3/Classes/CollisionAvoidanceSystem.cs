using System;
using System.Collections.Generic;
using System.Linq;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class CollisionAvoidanceSystem : ICollisionAvoidanceSystem
    {
        public event EventHandler<SeparationEventArgs> SeparationEvent;
        public event EventHandler<SeparationEventArgs> SeparationAvoidedEvent;
        private IAMSController _eventReceiver;
        private double _longitudeTolerance;
        private double _altitudeTolerance;
        private List<Tuple<Aircraft, Aircraft, SeparationEventArgs>> _collidingAircrafts = new List<Tuple<Aircraft, Aircraft, SeparationEventArgs>> { };

        public CollisionAvoidanceSystem(IAMSController eventReceiver, double longitudeTolerance, double altitudeTolerance)
        {
            _eventReceiver = eventReceiver;
            _eventReceiver.FilteredAircraftsEvent += ReceiverFilteredDataReady;
            _longitudeTolerance = longitudeTolerance;
            _altitudeTolerance = altitudeTolerance;
        }

        public void ReceiverFilteredDataReady(object sender, AircraftsFilteredEventArgs e)
        {
            CollisionWarning(e.filteredAircraft);
        }

        public void CollisionWarning(IDictionary<string, Aircraft> aircrafts)
        {
            for (int i = 0; i < aircrafts.Count; i++)
            {
                for (int j = i + 1; j < aircrafts.Count; j++)
                {
                    var ac1 = aircrafts.Values.ElementAt(i);
                    var ac2 = aircrafts.Values.ElementAt(j);
                    if (IsColliding(ac1, ac2))
                    {
                        if (!_collidingAircrafts.Any(x => x.Item1.Tag == ac1.Tag && x.Item2.Tag == ac2.Tag))
                        {
                            //This raises the collision event - but only the first time
                            var tempTuple = new Tuple<Aircraft, Aircraft, SeparationEventArgs>(ac1, ac2, new SeparationEventArgs(ac1, ac2));
                            _collidingAircrafts.Add(tempTuple);
                            SeparationEvent?.Invoke(this, tempTuple.Item3);
                        }
                    }
                    else
                    {
                        if (_collidingAircrafts.Any(x => x.Item1.Tag == ac1.Tag && x.Item2.Tag == ac2.Tag))
                        {
                            //This removes the collision event. 
                            var tempTuple =
                                _collidingAircrafts.Find(x => x.Item1.Tag == ac1.Tag && x.Item2.Tag == ac2.Tag);
                            SeparationAvoidedEvent?.Invoke(this, tempTuple.Item3);
                            _collidingAircrafts.Remove(tempTuple);
                            
                        }
                    }
                }
            }
        }

        public bool IsColliding(Aircraft ac1, Aircraft ac2)
        {
            double diffAltitude = Calculate.CalculateAltitudeDiff(ac1.Altitude, ac2.Altitude);
            double diffLongitude = Calculate.DistanceTo(ac1.XCoordinate, ac2.XCoordinate, ac1.YCoordinate, ac2.YCoordinate);
            return diffAltitude <= _altitudeTolerance && diffLongitude <= _longitudeTolerance;
        }
    }
}
