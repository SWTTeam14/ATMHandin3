using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;
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
            
        }
    }
}
