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

        private IAMSController _eventReceiver;
        private double _longitudeTolerance;
        private double _altitudeTolerance;
        private List<Tuple<Aircraft, Aircraft>> _collidingAircrafts = new List<Tuple<Aircraft, Aircraft>> { };

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
                            var tempTuple = new Tuple<Aircraft, Aircraft>(ac1, ac2);
                            _collidingAircrafts.Add(tempTuple);
                            SeparationEvent?.Invoke(this, new SeparationEventArgs(ac1, ac2));
                        }
                    }
                    else
                    {
                        if (_collidingAircrafts.Any(x => x.Item1.Tag == ac1.Tag && x.Item2.Tag == ac2.Tag))
                        {
                            _collidingAircrafts.RemoveAll(x => x.Item1.Tag == ac1.Tag && x.Item2.Tag == ac2.Tag);
                            //SeparationAvoidedEvent(this, new SeparationAvoidedEventArgs(ac1, ac2));
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
