using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ATMHandin3.Classes;
using ATMHandin3.Interfaces;
using NUnit.Framework;
using NSubstitute;
using TransponderReceiver;
using ATMHandin3.Classes;
using ATMHandin3.Events;

namespace Transponder.Test.Integration
{
    [TestFixture]
    public class TopDownStep1
    {
        private ITransponderReceiver transponder;
        private IAirspace airspace;
        private ICollisionAvoidanceSystem iCol;
        private IConsoleOutput consoleOutput;
        private ATMHandin3.Classes.Decoder decoder;

        private AMSController amsController;
        
        private int _nFilteredAircraftEvent = 0;
        private int _nTrackEnteredAirspaceEvent = 0;
        private int _nTrackLeftAirspaceEvent = 0;
        private int _nDataDecodedEvents = 0;
        
        [SetUp]
        public void SetUp()
        {
            
            iCol = Substitute.For<ICollisionAvoidanceSystem>();
            transponder = Substitute.For<ITransponderReceiver>();
            consoleOutput = Substitute.For<IConsoleOutput>();

            airspace = new Airspace(10000, 10000, 90000, 90000, 500, 20000);
            decoder = new Decoder(transponder);
            amsController = new AMSController(decoder, airspace);

            amsController.FilteredAircraftsEvent += (o, args) => { ++_nFilteredAircraftEvent; };
            amsController.TrackEnteredAirspaceEvent += (o, args) => { ++_nTrackEnteredAirspaceEvent; };
            amsController.TrackLeftAirspaceEvent += (o, args) => { ++_nTrackLeftAirspaceEvent; };
            decoder.DataDecodedEvent += (o, args) => { ++_nDataDecodedEvents; };
        }

        [Test]
        public void test_that_a_filteredAircraftEvent_is_raised_when_transponderData_is_ready()
        {
            _nFilteredAircraftEvent = 0;

            string testData1 = "ATR423;39044;12931;13999;20151006213456789";
            string testData2 = "GFD123;39045;12932;14000;20151006213456789";
            string testData3 = "MKD936;90001;12932;14000;20151006213456789";

            List<string> aircraftTestData = new List<string>();
            aircraftTestData.Add(testData1);
            aircraftTestData.Add(testData2);
            aircraftTestData.Add(testData3);

            transponder.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(aircraftTestData));
            Thread.Sleep(4000);

            //Assert.That(_nFilteredAircraftEvent, Is.EqualTo(1));
            Assert.AreEqual(1, _nFilteredAircraftEvent);
        }

        [Test]
        public void test_2_tracks_enter_airspace_and_1_leave()
        {
            _nTrackEnteredAirspaceEvent = 0;

            string testData1 = "ATR423;39045;12932;14000;20151006213456789";
            string testData2 = "GFD123;39045;12932;14000;20151006213456789";
            string testData3 = "MKD936;90001;12932;14000;20151006213456789";

            List<string> aircraftTestData = new List<string>();
            aircraftTestData.Add(testData1);
            aircraftTestData.Add(testData2);
            aircraftTestData.Add(testData3);

            transponder.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(aircraftTestData));

            Assert.That(_nTrackEnteredAirspaceEvent, Is.EqualTo(2));

            testData1 = "ATR423;40001;12932;14000;20151006213456789";
            testData2 = "GFD123;90001;12932;14000;20151006213456789";
            testData3 = "MKD936;90003;12932;14000;20151006213456789"; // this one never been inside
            
            aircraftTestData.Add(testData1);
            aircraftTestData.Add(testData2);
            aircraftTestData.Add(testData3);

            transponder.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(aircraftTestData));

            Assert.That(_nTrackLeftAirspaceEvent, Is.EqualTo(1));
        }

        [Test]
        public void test_if_decoder_sends_the_right_aircraft_data()
        {
            _nDataDecodedEvents = 0;
            List<Aircraft> aircrafts = new List<Aircraft>();
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 9000, 5000, new DateTime(1994, 09, 3));

            aircrafts.Add(a1);
            aircrafts.Add(a2);

            //DataDecodedEventArgs args = new DataDecodedEventArgs(aircrafts);
            decoder.DataDecodedEvent += Raise.EventWith(this, new DataDecodedEventArgs(aircrafts));

            Thread.Sleep(4000);
            Assert.AreEqual( _nDataDecodedEvents, 1);
            //Assert.That(1, Is.EqualTo(_nDataDecodedEvents));

        }
    }
}
