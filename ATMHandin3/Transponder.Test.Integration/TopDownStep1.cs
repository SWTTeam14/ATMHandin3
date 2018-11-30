using System;
using System.Collections.Generic;
using System.Threading;
using ATMHandin3.Classes;
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

            airspace = new Airspace(10000, 10000, 90000, 90000, 500, 20000);
            decoder = new Decoder(transponder);
            amsController = new AMSController(decoder, airspace);

            amsController.FilteredAircraftsEvent += (o, args) => { ++_nFilteredAircraftEvent; };
            amsController.TrackEnteredAirspaceEvent += (o, args) => { ++_nTrackEnteredAirspaceEvent; };
            amsController.TrackLeftAirspaceEvent += (o, args) => { ++_nTrackLeftAirspaceEvent; };
            decoder.DataDecodedEvent += (o, args) => { ++_nDataDecodedEvent; };
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

            transponder.TransponderDataReady +=
                Raise.EventWith(this, new RawTransponderDataEventArgs(aircraftTestData));
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

            transponder.TransponderDataReady +=
                Raise.EventWith(this, new RawTransponderDataEventArgs(aircraftTestData));

            Assert.That(_nTrackEnteredAirspaceEvent, Is.EqualTo(2));

            testData1 = "ATR423;40001;12932;14000;20151006213456789";
            testData2 = "GFD123;90001;12932;14000;20151006213456789";
            testData3 = "MKD936;90003;12932;14000;20151006213456789"; // this one never been inside

            aircraftTestData.Add(testData1);
            aircraftTestData.Add(testData2);
            aircraftTestData.Add(testData3);

            transponder.TransponderDataReady +=
                Raise.EventWith(this, new RawTransponderDataEventArgs(aircraftTestData));

            Assert.That(_nTrackLeftAirspaceEvent, Is.EqualTo(1));
        }

        [Test]
        public void test_that_amscontroller_receives_correct_data_from_decoder()
        {
            _nDataDecodedEvent = 0;

            List<string> aircraList = new List<string>();

            string testData1 = "ttt;20000;20000;5000;19940903000000000";
            string testData2 = "yyy;20000;20000;4500;19940903000000000";

            aircraList.Add(testData1);
            aircraList.Add(testData2);

            transponder.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(aircraList));

            Assert.AreEqual(_nDataDecodedEvent, 1);

            Dictionary<string, Aircraft> airctaftsToTest = new Dictionary<string, Aircraft>();

            Aircraft a1 = new Aircraft("ttt", 20000, 20000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 20000, 20000, 4500, new DateTime(1994, 09, 3));

            airctaftsToTest.Add("ttt", a1);
            airctaftsToTest.Add("yyy", a2);

            CollectionAssert.AreEquivalent(amsController.filteredAircrafts.Keys, airctaftsToTest.Keys);
        }
    }
}
