using System;
using ATMHandin3.Classes;
using ATMHandin3.EventManager;
using NUnit.Framework;

namespace Transponder.Test.Unit
{
    public class EventManagerTest
    {
        private EventManager _uut;
        private Aircraft air;
        [SetUp]
        public void Setup()
        {
            _uut = new EventManager();
            air = new Aircraft("ATR423", 39045, 12932, 14000, new DateTime());
        }

        [Test]
        public void TestTimedEvent()
        {
            Assert.AreEqual(_uut.EventList.Count, 0);
            TrackEnteredAirspaceEvent tEvent = new TrackEnteredAirspaceEvent(air);
            _uut.AddEvent(tEvent);
            Assert.AreEqual(_uut.EventList.Count, 1);

        }
    }
}
