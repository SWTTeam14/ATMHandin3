using System;
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
        private int _nDataDecodedEvents = 0;

        [SetUp]
        public void Setup()
        {
            _fakeTransponderReceiver = Substitute.For<ITransponderReceiver>();
            _fakeDecoder = Substitute.For<IDecoder>();
            _uut = new Decoder(_fakeTransponderReceiver);
            _uut.DataDecodedEvent += (o, args) => { ++_nDataDecodedEvents; };
        }

        [Test]
        public void TestReception()
        {
            List<string> testData = new List<string>();
            testData.Add("ATR423;39045;12932;14000;20151006213456789");
            testData.Add("BCD123;10005;85890;12000;20151006213456789");
            testData.Add("XYZ987;25059;75654;4000;20151006213456789");

            _fakeTransponderReceiver.TransponderDataReady +=
                Raise.EventWith(_fakeTransponderReceiver, new RawTransponderDataEventArgs(testData));

            Assert.AreEqual(_nDataDecodedEvents, 1);
        }

        [Test]
        public void TestConvertTime()
        {
            string testTime = "20151006213456789";
            DateTime dateTime = DateTime.ParseExact(testTime, "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            Assert.That(_uut.ConvertTime(testTime), Is.EqualTo(dateTime));
        }

        [Test]
        public void TestConvertData()
        {
            string testData = "ATR423;39045;12932;14000;20151006213456789";
            DateTime dateTime = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            Aircraft aircraft = new Aircraft("ATR423", 39045, 12932, 14000, dateTime);

            //Assert.That(_uut.ConvertData(testData), Is.EqualTo(aircraft));
            Assert.AreEqual(_uut.ConvertData(testData), aircraft);
        }
    }
}

