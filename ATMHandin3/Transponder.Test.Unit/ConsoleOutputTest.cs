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
    [TestFixture]
    public class ConsoleOutputTest
    {
        private ConsoleOutput _uut;
        private IOutput output;
        private IAMSController amsController;
        private ICollisionAvoidanceSystem collisionAvoidanceSystem;

        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            amsController = Substitute.For<IAMSController>();
            collisionAvoidanceSystem = Substitute.For<ICollisionAvoidanceSystem>();
            _uut = new ConsoleOutput(amsController, collisionAvoidanceSystem, output);
        }

        [Test]
        public void 
    }
}
