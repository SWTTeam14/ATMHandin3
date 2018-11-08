using System;
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

        private IAMSController _fakeAmsController;
        private AMSController _uut;

        private int _nFilteredAircraftEvent = 0;
        private int _nTrackEnteredAirspaceEvent = 0;
        private int _nTrackLeftAirspaceEvent = 0;


        [SetUp]
        public void Setup()
        {
            _fakeDecoder = Substitute.For<IDecoder>();
            _fakeAirspace = Substitute.For<IAirspace>();

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

            testData.Add(aircraft1);
            testData.Add(aircraft2);
            
            _fakeDecoder.DataDecodedEvent += Raise.EventWith(_fakeDecoder, new DataDecodedEventArgs(testData));

            Assert.AreEqual(_nFilteredAircraftEvent, 1);
            
            
        }

        
    }
}
