using System.Collections.Generic;
using ATMHandin3.Interfaces;
using ATMHandin3.Classes;
using NUnit.Framework;
using NSubstitute;
using TransponderReceiver;

namespace Transponder.Test.Unit
{
    public class DecoderTest
    {
        private ITransponderReceiver _fakeTransponderReceiver;
        private Decoder _uut;
        private IDecoder _fakeDecoder;


        [SetUp]
        public void Setup()
        {
            _fakeTransponderReceiver = Substitute.For<ITransponderReceiver>();
            _fakeDecoder = Substitute.For<IDecoder>();
            _uut = new Decoder(_fakeTransponderReceiver);

            _fakeTransponderReceiver.TransponderDataReady += (o, args) => { };
        }

        [Test]
        public void TestConvertData()
        {
            string testData = "ATR423;39045;12932;14000;20151006213456789";

            Aircraft ac = _fakeDecoder.convertData(testData);

            Assert.That(_fakeDecoder.convertData(testData), Is.EqualTo(ac));
        }
    }
}

