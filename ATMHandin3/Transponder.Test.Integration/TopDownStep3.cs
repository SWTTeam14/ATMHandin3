using System.Collections.Generic;
using System.IO;
using ATMHandin3.Interfaces;
using ATMHandin3.Classes;
using NUnit.Framework;
using NSubstitute;
using TransponderReceiver;

namespace Transponder.Test.Integration
{
    public class TopDownStep3
    {
        private int _nSeperationEvent = 0;
        
        private ITransponderReceiver _fakeTransponderReceiver;
        private IOutput _fakeOutput;

        private Decoder _realDecoder;
        private AMSController _realAmsController;
        private CollisionAvoidanceSystem _realAvoidanceSystem;
        private ConsoleOutput _realConsoleOutput;
        private Logger _realLogger;
        private Airspace _realAirspace;
        private IFileLogger _fakeIFileLogger;
        
        [SetUp]
        public void Setup()
        {
            _fakeTransponderReceiver = Substitute.For<ITransponderReceiver>();
            _fakeOutput = Substitute.For<IOutput>();
            _fakeIFileLogger = Substitute.For<IFileLogger>();

            _realAirspace = new Airspace(10000, 10000, 90000, 90000, 500, 20000)
            {
                South = 10000,
                West = 10000,
                North = 90000,
                East = 90000,
                LowerAltitude = 500,
                UpperAltitude = 20000
            };

            _realDecoder = new Decoder(_fakeTransponderReceiver);
            _realAmsController = new AMSController(_realDecoder, _realAirspace);
            _realAvoidanceSystem = new CollisionAvoidanceSystem(_realAmsController, 5000, 300);
            
            _realLogger = new Logger(_realAvoidanceSystem);
            _realConsoleOutput = new ConsoleOutput(_realAmsController, _realAvoidanceSystem, _fakeOutput);
            
            _realAvoidanceSystem.SeparationEvent += (o, args) => { ++_nSeperationEvent; };
        }

        [Test]
        public void Test_that_fileLogger_gets_the_correct_data_from_avoidance_system()
        {
            _nSeperationEvent = 0;

            List<string> aircraftList = new List<string>();

            string testData1 = "ttt;20000;20000;5000;19940903000000000";
            string testData2 = "yyy;20000;20000;5000;19940903000000000";

            aircraftList.Add(testData1);
            aircraftList.Add(testData2);

            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(aircraftList));

            Assert.AreEqual(_nSeperationEvent, 1);

            Assert.That(_realLogger.CheckIfFileExists(), Is.EqualTo(true));

            string str = File.ReadAllText("SeparationEventLogFile.txt");

            Assert.That(str, Is.EqualTo("Collision event; ttt; yyy; 03-09-1994 00:00:00\r\n"));
        }

        [Test]
        public void Test_that_fileLogger_does_not_write_anything_on_no_seperation_event_EG_file_is_not_found()
        {
            _nSeperationEvent = 0;

            List<string> aircraftList = new List<string>();

            string testData1 = "ttt;90000;20000;15000;19940903000000000";
            string testData2 = "yyy;20000;20000;5000;19940903000000000";

            aircraftList.Add(testData1);
            aircraftList.Add(testData2);

            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(aircraftList));

           Assert.That(_realLogger.CheckIfFileExists(), Is.EqualTo(false));

        }

        [Test]
        public void Test_that_output_gets_the_correct_data_from_avoidance_system_through_consoleOutput()
        {
            _nSeperationEvent = 0;

            List<string> aircraftList = new List<string>();

            string testData1 = "ttt;20000;20000;5000;19940903000000000";
            string testData2 = "yyy;20000;20000;5000;19940903000000000";

            aircraftList.Add(testData1);
            aircraftList.Add(testData2);

            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(this, new RawTransponderDataEventArgs(aircraftList));

            Assert.AreEqual(_nSeperationEvent, 1);

            _fakeOutput.Received().OutputWriteline(Arg.Is<string>(str =>
                str.Contains("AIRCRAFTS ARE COLLIDING")));
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

        //CoalitionAvoidanceSystem
        [Test]
        public void TestSeparationEventIsNotRaised()
        {
            _nSeperationEvent = 0;

            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            testData.Add("BCD123;10005;85890;12000;20151006213456789");
            testData.Add("XYZ987;25059;75654;4000;20151006213456789");

            RawTransponderDataEventArgs arg = new RawTransponderDataEventArgs(testData);
            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderReceiver, arg);

            //We expect based on the previous assert result, that this will result in a separation event.
            Assert.AreEqual(_nSeperationEvent, 0);
        }

        //CoalitionAvoidanceSystem
        [Test]
        public void TestSeparationEventIsRaised()
        {
            _nSeperationEvent = 0;

            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            testData.Add("BCD123;10005;85890;12000;20151006213456789");
            testData.Add("XYZ987;10005;85890;12000;20151006213456789");

            RawTransponderDataEventArgs arg = new RawTransponderDataEventArgs(testData);
            _fakeTransponderReceiver.TransponderDataReady += Raise.EventWith(_fakeTransponderReceiver, arg);

            //We expect based on the previous assert result, that this will result in a separation event.
            Assert.AreEqual(_nSeperationEvent, 1);
        }
    }
}