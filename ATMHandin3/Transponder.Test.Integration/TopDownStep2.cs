using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Interfaces;
using ATMHandin3.Classes;
using ATMHandin3.Events;
using NUnit.Framework;
using NSubstitute;
using TransponderReceiver;

namespace Transponder.Test.Integration
{
    public class TopDownStep2
    {
        private int _nSeparationEvents;

        private ITransponderReceiver _fakeTransponderReceiver;
        
        private ATMHandin3.Classes.Decoder _realDecoder;
        private AMSController _realAmsController;
        private CollisionAvoidanceSystem _realAvoidanceSystem;
        private Airspace _realAirspace;

        [SetUp]
        public void Setup()
        {
            _fakeTransponderReceiver = Substitute.For<ITransponderReceiver>();
            
            _realAirspace = new Airspace(10000, 10000, 90000, 90000, 500, 20000);

            _realDecoder = new ATMHandin3.Classes.Decoder(_fakeTransponderReceiver);
            _realAmsController = new AMSController(_realDecoder, _realAirspace);
            _realAvoidanceSystem = new CollisionAvoidanceSystem(_realAmsController, 5000, 300);
            
            _realAvoidanceSystem.SeparationEvent += (o, args) => { ++_nSeparationEvents; };
        }

        [Test]
        public void TestSeparationEventIsRaised()
        {
            _nSeparationEvents = 0;
 
            List<Aircraft> testList = new List<Aircraft>();

            DateTime dateTime1 = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            Aircraft aircraft1 = new Aircraft("ATR423", 39045, 12932, 14000, dateTime1);
            DateTime dateTime2 = DateTime.ParseExact("20151006213558789", "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            Aircraft aircraft2 = new Aircraft("BTR700", 19100, 23000, 1000, dateTime2);
            Aircraft aircraft3 = new Aircraft("CTR100", 19100, 23000, 1000, dateTime2);
            
            testList.Add(aircraft1);
            testList.Add(aircraft2);
            testList.Add(aircraft3);

            DataDecodedEventArgs decodedArg = new DataDecodedEventArgs(testList);
            _realDecoder.DataDecodedEvent += Raise.EventWith(_realDecoder, decodedArg);

            _realAmsController.DataDecodedEventHandler(_realDecoder, decodedArg);

            _realAvoidanceSystem.CollisionWarning(_realAmsController.filteredAircrafts);
            //We expect based on the previous assert result, that this will result in a separation event.
            Assert.AreEqual(_nSeparationEvents, 2);
        }

        [Test]
        public void TestSeparationEventIsNotRaised()
        {
            _nSeparationEvents = 0;

            List<Aircraft> testList = new List<Aircraft>();

            DateTime dateTime1 = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            Aircraft aircraft1 = new Aircraft("ATR423", 50000, 50000, 9000, dateTime1);
            Aircraft aircraft2 = new Aircraft("BTR700", 30000, 30000, 5000, dateTime1);
            Aircraft aircraft3 = new Aircraft("CTR100", 19100, 23000, 1000, dateTime1);

            testList.Add(aircraft1);
            testList.Add(aircraft2);
            testList.Add(aircraft3);

            DataDecodedEventArgs decodedArg = new DataDecodedEventArgs(testList);
            _realDecoder.DataDecodedEvent += Raise.EventWith(_realDecoder, decodedArg);

            _realAmsController.DataDecodedEventHandler(_realDecoder, decodedArg);

            _realAvoidanceSystem.CollisionWarning(_realAmsController.filteredAircrafts);
            //We expect based on the previous assert result, that this will result in a separation event.
            Assert.AreEqual(_nSeparationEvents, 0);
        }

        //[Test]
        //public void TestConsoleOutputIsCorrect()
        //{
        //    
        //}
    }
}
