﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;
using ATMHandin3.Interfaces;
using ATMHandin3.Events;
using NUnit.Framework;
using NSubstitute;
using TransponderReceiver;

namespace Transponder.Test.Unit
{
    public class AMSControllerTest
    {
        private IDecoder _fakeDecoder;
        private IAirspace _fakeAirspace;
        
        private AMSController _uut;
        private Airspace _testAirspace;

        private int _nFilteredAircraftEvent = 0;
        private int _nTrackEnteredAirspaceEvent = 0;
        private int _nTrackLeftAirspaceEvent = 0;


        [SetUp]
        public void Setup()
        {
            _fakeDecoder = Substitute.For<IDecoder>();
            _fakeAirspace = Substitute.For<IAirspace>();

            _testAirspace = new Airspace(10000, 10000, 90000, 90000, 500, 20000)
            {
                South = 10000,
                West = 10000,
                North = 90000,
                East = 90000,
                LowerAltitude = 500,
                UpperAltitude = 20000
            };

            _uut = new AMSController(_fakeDecoder, _fakeAirspace);

            _uut.FilteredAircraftsEvent += (o, args) => { ++_nFilteredAircraftEvent; };
            _uut.TrackEnteredAirspaceEvent += (o, args) => { ++_nTrackEnteredAirspaceEvent; };
            _uut.TrackLeftAirspaceEvent += (o, args) => { ++_nTrackLeftAirspaceEvent; };

        }

        [Test]
        public void TestReception()
        {
            List<Aircraft> testData = new List<Aircraft>();

            DateTime dateTime1 = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            Aircraft aircraft1 = new Aircraft("ATR423", 39045, 12932, 14000, dateTime1);
            DateTime dateTime2 = DateTime.ParseExact("20151006213558789", "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            Aircraft aircraft2 = new Aircraft("BTR700", 19100, 23000, 1000, dateTime2);

            _fakeAirspace.South = 0;
            _fakeAirspace.West = 0;
            _fakeAirspace.North = 30000;
            _fakeAirspace.East = 30000;
            _fakeAirspace.LowerAltitude = 500;
            _fakeAirspace.UpperAltitude = 15000;
            
            testData.Add(aircraft1);
            testData.Add(aircraft2);
            
            _fakeDecoder.DataDecodedEvent += Raise.EventWith(_fakeDecoder, new DataDecodedEventArgs(testData));
            
            Assert.AreEqual(_nFilteredAircraftEvent, 1);
            Assert.AreEqual(_nTrackEnteredAirspaceEvent, 1);

            _uut.filteredAircrafts["BTR700"].XCoordinate = 35000;

            _fakeDecoder.DataDecodedEvent += Raise.EventWith(_fakeDecoder, new DataDecodedEventArgs(testData));

            Assert.AreEqual(_nTrackLeftAirspaceEvent, 1);
        }

        
    }
}
