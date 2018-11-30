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
        private FileLogger _realFileLogger;
        private Airspace _realAirspace;
        
        [SetUp]
        public void Setup()
        {
            _fakeTransponderReceiver = Substitute.For<ITransponderReceiver>();
            _fakeOutput = Substitute.For<IOutput>();

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
            _realFileLogger = new FileLogger(_realAvoidanceSystem);
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

            Assert.That(_realFileLogger.CheckIfFileExists(), Is.EqualTo(true));

            string str = File.ReadAllText("SeparationEventLogFile.txt");

            Assert.That(str, Is.EqualTo("Collision event; ttt; yyy; 03-09-1994 00:00:00\r\n"));
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
    }
}