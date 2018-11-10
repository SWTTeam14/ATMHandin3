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

            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            testData.Add("BCD123;10005;85890;12000;20151006213456789");
            testData.Add("XYZ987;10005;85890;12000;20151006213456789");

            RawTransponderDataEventArgs arg = new RawTransponderDataEventArgs(testData);
            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderReceiver, arg);
            
            //We expect based on the previous assert result, that this will result in a separation event.
            Assert.AreEqual(_nSeparationEvents, 1);
        }

        [Test]
        public void TestSeparationEventIsNotRaised()
        {
            _nSeparationEvents = 0;

            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            testData.Add("BCD123;10005;85890;12000;20151006213456789");
            testData.Add("XYZ987;25059;75654;4000;20151006213456789");

            RawTransponderDataEventArgs arg = new RawTransponderDataEventArgs(testData);
            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderReceiver, arg);
            
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