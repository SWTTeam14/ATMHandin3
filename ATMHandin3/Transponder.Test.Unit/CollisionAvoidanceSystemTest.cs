using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Transponder.Test.Unit
{
    public class CollisionAvoidanceSystemTest
    {
        private CollisionAvoidanceSystem _uut;
        private IAMSController amsController;
        private int longtitude;
        private int altitude;

        private int _nSeparationEvents;

        [SetUp]
        public void Setup()
        {
            
            amsController = Substitute.For<IAMSController>();
            longtitude = 5000;
            altitude = 300;
            _uut = new CollisionAvoidanceSystem(amsController, longtitude, altitude);

            _uut.SeparationEvent += (o, args) => { ++_nSeparationEvents; };
        }

        [Test]
        public void IsColliding_method_returns_true_when_aircrafts_are_colliding()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 9000, 5000, new DateTime(1994, 09, 3));

            Assert.That(true, Is.EqualTo(_uut.IsColliding(a1, a2)));
        }

        [Test]
        public void IsColliding_method_returns_false_when_aircrafts_are_not_colliding_by_altitutde()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 9000, 4500, new DateTime(1994, 09, 3));

            Assert.That(false, Is.EqualTo(_uut.IsColliding(a1, a2)));
        }

        [Test]
        public void IsColliding_method_returns_false_when_aircrafts_are_not_colliding_by_longtitude()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 90000, 90000, 5000, new DateTime(1994, 09, 3));

            Assert.That(false, Is.EqualTo(_uut.IsColliding(a1, a2)));
        }

        [Test]
        public void SeperationEvent_is_raised_once_when_2_aircrafts_are_colliding()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Dictionary<string, Aircraft> aircrafts = new Dictionary<string, Aircraft>();

            aircrafts.Add("ttt", a1);
            aircrafts.Add("yyy", a2);

            amsController.FilteredAircraftsEvent += Raise.EventWith(this, new AircraftsFilteredEventArgs(aircrafts));
            Assert.AreEqual(1,_nSeparationEvents);
        }

        [Test]
        public void SeperationEvent_is_not_raised_when_2_aircrafts_are_not_colliding()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 70000, 4500, new DateTime(1994, 09, 3));
            Dictionary<string, Aircraft> aircrafts = new Dictionary<string, Aircraft>();

            aircrafts.Add("ttt", a1);
            aircrafts.Add("yyy", a2);

            amsController.FilteredAircraftsEvent += Raise.EventWith(this, new AircraftsFilteredEventArgs(aircrafts));
            Assert.AreEqual(0, _nSeparationEvents);
        }
    }
}
