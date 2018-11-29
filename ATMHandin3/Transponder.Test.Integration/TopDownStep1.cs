using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ATMHandin3.Classes;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;
using NUnit.Framework;
using NSubstitute;
using TransponderReceiver;

namespace Transponder.Test.Integration
{
    [TestFixture]
    public class TopDownStep1
    {
        private ITransponderReceiver transponder;
        private IAirspace airspace;
        private ICollisionAvoidanceSystem iCol;
        private IOutput output;

        private Decoder decoder;
        private AMSController amsController;
        
        private int _nFilteredAircraftEvent = 0;
        private int _nTrackEnteredAirspaceEvent = 0;
        private int _nTrackLeftAirspaceEvent = 0;
        private int _nDataDecodedEvent = 0;
        
        [SetUp]
        public void SetUp()
        {
            transponder = Substitute.For<ITransponderReceiver>();
            output = Substitute.For<IOutput>();

            airspace = new Airspace(10000, 10000, 90000, 90000, 500, 20000);
            decoder = new Decoder(transponder);
            amsController = new AMSController(decoder, airspace);
            
            amsController.FilteredAircraftsEvent += (o, args) => { ++_nFilteredAircraftEvent; };
            amsController.TrackEnteredAirspaceEvent += (o, args) => { ++_nTrackEnteredAirspaceEvent; };
            amsController.TrackLeftAirspaceEvent += (o, args) => { ++_nTrackLeftAirspaceEvent; };
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
            
            Assert.AreEqual(1, _nFilteredAircraftEvent);


        }

        [Test]
        public void test_2_tracks_enter_airspace_and_1_leave()
        {
            _nTrackEnteredAirspaceEvent = 0;
            _nDataDecodedEvent = 0;

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
    }
}
