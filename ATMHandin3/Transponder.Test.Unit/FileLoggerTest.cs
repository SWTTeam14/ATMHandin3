using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;
using NUnit.Framework;
using NSubstitute;


namespace Transponder.Test.Unit
{
    public class FileLoggerTest
    {
        private ICollisionAvoidanceSystem _fakeCollisionAvoidanceSystem;
        private FileLogger _uut;

        [SetUp]
        public void Setup()
        {
            _fakeCollisionAvoidanceSystem = Substitute.For<ICollisionAvoidanceSystem>();
            _uut = new FileLogger(_fakeCollisionAvoidanceSystem);
        }

        [Test]
        public void Test_if_log_file_exist()
        {
            string testTime1 = "20151006213456789";
            string testTime2 = "20151006213456789";
            DateTime dateTime1 = DateTime.ParseExact(testTime1, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dateTime2 = DateTime.ParseExact(testTime2, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);

            Aircraft air1 = new Aircraft("ATR423", 39045, 12932, 14000, dateTime1);
            Aircraft air2 = new Aircraft("BCD123", 10005, 85890, 12000, dateTime2);

            _fakeCollisionAvoidanceSystem.SeparationEvent +=
                Raise.EventWith(_fakeCollisionAvoidanceSystem, new SeparationEventArgs(air1, air2));
            
            Assert.That(_uut.CheckIfFileExists(), Is.EqualTo(true));
        }


        [Test]
        public void Logging_correct_data()
        {
            string testTime1 = "20151006213456789";
            string testTime2 = "20151006213456789";
            DateTime dateTime1 = DateTime.ParseExact(testTime1, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dateTime2 = DateTime.ParseExact(testTime2, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);

            Aircraft air1 = new Aircraft("ATR423", 39045, 12932, 14000, dateTime1);
            Aircraft air2 = new Aircraft("BCD123", 10005, 85890, 12000, dateTime2);

            _fakeCollisionAvoidanceSystem.SeparationEvent +=
                Raise.EventWith(_fakeCollisionAvoidanceSystem, new SeparationEventArgs(air1, air2));

            Assert.That(_uut.CheckIfFileExists(), Is.EqualTo(true));

            string str = File.ReadAllText("SeparationEventLogFile.txt");

            Assert.That(str, Is.EqualTo("Collision event; ATR423; BCD123; 06-10-2015 21:34:56\r\n"));
        }

        [Test]
        public void Logging_correct_data_multiple_events()
        {
            string testTime1 = "20151006213456789";
            string testTime2 = "20151006213456789";
            DateTime dateTime1 = DateTime.ParseExact(testTime1, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dateTime2 = DateTime.ParseExact(testTime2, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dateTime3 = DateTime.ParseExact(testTime2, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);

            Aircraft air1 = new Aircraft("ATR423", 39045, 12932, 14000, dateTime1);
            Aircraft air2 = new Aircraft("BCD123", 10005, 85890, 12000, dateTime2);
            Aircraft air3 = new Aircraft("HCD123", 20000, 90890, 13000, dateTime3);

            _fakeCollisionAvoidanceSystem.SeparationEvent +=
                Raise.EventWith(_fakeCollisionAvoidanceSystem, new SeparationEventArgs(air1, air2));

            Assert.That(_uut.CheckIfFileExists(), Is.EqualTo(true));

            string str = File.ReadAllText("SeparationEventLogFile.txt");

            Assert.That(str, Is.EqualTo("Collision event; ATR423; BCD123; 06-10-2015 21:34:56\r\n"));

            _fakeCollisionAvoidanceSystem.SeparationEvent +=
                Raise.EventWith(_fakeCollisionAvoidanceSystem, new SeparationEventArgs(air1, air3));

            str = File.ReadAllText("SeparationEventLogFile.txt");

            Assert.IsTrue(str.Contains("HCD123"));
        }
    }
}
