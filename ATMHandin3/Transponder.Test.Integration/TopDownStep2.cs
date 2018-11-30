using System.Collections.Generic;
using ATMHandin3.Interfaces;
using ATMHandin3.Classes;
using NUnit.Framework;
using NSubstitute;
using TransponderReceiver;

namespace Transponder.Test.Integration
{
    public class TopDownStep2
    {
        private int _nSeparationEvents;

        private ITransponderReceiver _fakeTransponderReceiver;
        private IOutput _fakeOutput;

        private Decoder _realDecoder;
        private AMSController _realAmsController;
        private CollisionAvoidanceSystem _realAvoidanceSystem;
        private Airspace _realAirspace;
        private ConsoleOutput _realConsoleOutput;

        [SetUp]
        public void Setup()
        {
            _fakeTransponderReceiver = Substitute.For<ITransponderReceiver>();
            _fakeOutput = Substitute.For<IOutput>();

            _realAirspace = new Airspace(10000, 10000, 90000, 90000, 500, 20000);

            _realAirspace = new Airspace(10000, 10000, 90000, 90000, 500, 20000);
            _realDecoder = new Decoder(_fakeTransponderReceiver);

            // UNDER TEST
            _realAmsController = new AMSController(_realDecoder, _realAirspace);
            _realAvoidanceSystem = new CollisionAvoidanceSystem(_realAmsController, 5000, 300);
            //Tests will not work, if ConsoleOutput is not present.
            _realConsoleOutput = new ConsoleOutput(_realAmsController, _realAvoidanceSystem, _fakeOutput);

            _realAvoidanceSystem.SeparationEvent += (o, args) => { ++_nSeparationEvents; };
        }
        
        //CoalitionAvoidanceSystem
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

        //CoalitionAvoidanceSystem
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

        [Test]
        public void Test_the_correct_output_()
        {

            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            testData.Add("BCD123;10005;85890;12000;20151006213456789");
            testData.Add("XYZ987;25059;75654;4000;20151006213456789");

            RawTransponderDataEventArgs arg = new RawTransponderDataEventArgs(testData);
            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderReceiver, arg);
            
            _fakeOutput.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("ATR423")));
            _fakeOutput.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("BCD123")));
            _fakeOutput.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("XYZ987")));
        }

        
        [Test]
        public void Test_that_1_set_of_aircrafts_are_colliding___Interface_between_COA_and_ConsoleOutput()
        {
            List<string> testData = new List<string>();
            testData.Add("ATR423;10006;12933;12001;20151006213456789");
            testData.Add("BCD123;10005;12932;12000;20151006213456789");
            testData.Add("XYZ987;25059;75654;4000;20151006213456789");

            RawTransponderDataEventArgs arg = new RawTransponderDataEventArgs(testData);
            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderReceiver, arg);

            _fakeOutput.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("Number of colliding aircrafts : 1")));
        }

        [Test]
        public void Test_that_3_aircrafts_are_inside_airspace()
        {
            List<string> testData = new List<string>();
            testData.Add("ATR423;10006;12933;12001;20151006213456789");
            testData.Add("BCD123;10005;12932;12000;20151006213456789");
            testData.Add("XYZ987;25059;75654;4000;20151006213456789");

            RawTransponderDataEventArgs arg = new RawTransponderDataEventArgs(testData);
            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderReceiver, arg);

            _fakeOutput.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("Number of airplanes inside airspace : 3")));
            
        }

   


    }
}