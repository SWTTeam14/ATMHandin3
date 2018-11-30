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
        private IFileLogger _fakeFileLogger;

        private ATMHandin3.Classes.Decoder _realDecoder;
        private AMSController _realAmsController;
        private CollisionAvoidanceSystem _realAvoidanceSystem;
        private Airspace _realAirspace;
        private ConsoleOutput _realConsoleOutput;

        [SetUp]
        public void Setup()
        {
            _fakeTransponderReceiver = Substitute.For<ITransponderReceiver>();
            _fakeOutput = Substitute.For<IOutput>();
            _fakeFileLogger = Substitute.For<IFileLogger>();

            _realAirspace = new Airspace(10000, 10000, 90000, 90000, 500, 20000);

            _realAirspace = new Airspace(10000, 10000, 90000, 90000, 500, 20000);
            _realDecoder = new ATMHandin3.Classes.Decoder(_fakeTransponderReceiver);

            // UNDER TEST
            _realAmsController = new AMSController(_realDecoder, _realAirspace);
            _realAvoidanceSystem = new CollisionAvoidanceSystem(_realAmsController, 5000, 300);
            _realConsoleOutput = new ConsoleOutput(_realAmsController, _realAvoidanceSystem, _fakeOutput);

            _realAvoidanceSystem.SeparationEvent += (o, args) => { ++_nSeparationEvents; };
        }

        // IF between AMScontroller and output.
        [Test]
        public void Test_the_correct_output()
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

        [Test]
        public void Test_that_3_aircrafts_are_inside_airspace_low_corner_case()
        {
            List<string> testData = new List<string>();
            testData.Add("ATR423;10000;10000;5000;20151006213456789");
            testData.Add("BCD123;10000;10000;5000;20151006213456789");
            testData.Add("XYZ987;25059;75654;4000;20151006213456789");

            RawTransponderDataEventArgs arg = new RawTransponderDataEventArgs(testData);
            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderReceiver, arg);

            _fakeOutput.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("Number of airplanes inside airspace : 3")));
        }

        [Test]
        public void Test_that_2_aircrafts_are_inside_airspace_and_1_not_low_corner_case()
        {
            List<string> testData = new List<string>();
            testData.Add("ATR423;9999;10000;5000;20151006213456789");
            testData.Add("BCD123;10000;10000;5000;20151006213456789");
            testData.Add("XYZ987;25059;75654;4000;20151006213456789");

            RawTransponderDataEventArgs arg = new RawTransponderDataEventArgs(testData);
            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderReceiver, arg);

            _fakeOutput.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("Number of airplanes inside airspace : 2")));
        }

        [Test]
        public void Test_that_1_aircrafts_are_inside_airspace_and_2_not_because_of_altitute_and_y_coordinate()
        {
            List<string> testData = new List<string>();
            testData.Add("ATR423;10000;10000;499;20151006213456789");
            testData.Add("BCD123;89000;9999;5000;20151006213456789");
            testData.Add("XYZ987;25059;75654;5500;20151006213456789");

            RawTransponderDataEventArgs arg = new RawTransponderDataEventArgs(testData);
            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderReceiver, arg);

            _fakeOutput.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("Number of airplanes inside airspace : 1")));
        }

        [Test]
        public void Test_that_1_aircrafts_are_inside_airspace_and_2_not_because_of_x_coordinate_and_upper_altitude()
        {
            List<string> testData = new List<string>();
            testData.Add("ATR423;5000;10000;5000;20151006213456789");
            testData.Add("BCD123;89000;9999;10001;20151006213456789");
            testData.Add("XYZ987;25059;75654;5500;20151006213456789");

            RawTransponderDataEventArgs arg = new RawTransponderDataEventArgs(testData);
            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderReceiver, arg);

            _fakeOutput.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("Number of airplanes inside airspace : 1")));
        }







    }
}