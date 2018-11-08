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
        private IFileLogger _fakeFileLogger;
        private FileLogger _uut;

        [SetUp]
        public void Setup()
        {
            _fakeCollisionAvoidanceSystem = Substitute.For<ICollisionAvoidanceSystem>();
            _fakeFileLogger = Substitute.For<IFileLogger>();

            _uut = new FileLogger(_fakeCollisionAvoidanceSystem);
        }

        [Test]
        public void TestSeparationLog()
        {
            string testTime1 = "20151006213456789";
            string testTime2 = "20151006213456789";
            DateTime dateTime1 = DateTime.ParseExact(testTime1, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dateTime2 = DateTime.ParseExact(testTime2, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);

            Aircraft air1 = new Aircraft("ATR423", 39045, 12932, 14000, dateTime1);
            Aircraft air2 = new Aircraft("BCD123", 10005, 85890, 12000, dateTime2);

            _fakeCollisionAvoidanceSystem.SeparationEvent +=
                Raise.EventWith(_fakeCollisionAvoidanceSystem, new SeparationEventArgs(air1, air2));
            
            
        }
    }
}
