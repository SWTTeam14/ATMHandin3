﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;
using NUnit.Framework;
using NSubstitute;

namespace Transponder.Test.Unit
{
    public class CalculateTest
    {
        private DateTime _tmpTime1;
        private DateTime _tmpTime2;

        [TestCase(14000, 12000, 57000, 52000)]
        public void TestCalculateVelocity(double a, double b, double c, double d)
        {
            _tmpTime1 = new DateTime(2018, 07, 09, 20, 40, 10, 902);
            _tmpTime2 = new DateTime(2018, 07, 09, 20, 40, 20, 902);

            Assert.AreEqual(Calculate.CalculateVelocity(a, b, c, d, _tmpTime1, _tmpTime2), 538.51648071345039);
        }

        [TestCase(0, 2, 0, 1, 63.4349488)]
        [TestCase(0, 2, 0, -1, 180 - 63.4349488)]
        [TestCase(0, -2, 0, -1, 180 + 63.4349488)]
        [TestCase(0, -2, 0, 1, (180 - 63.4349488) + 180)]
        public void TestCalculateAngle(double x1, double x2, double y1, double y2, double angle)
        {
            Assert.That(Calculate.CalculateAngle(x1, x2, y1, y2), Is.InRange(angle - 0.1, angle + 0.1));
        }

        [TestCase(11944, 43486, 12510, 45348)]
        public void TestDistanceCalculation(double x1Coor, double y1Coor, double x2Coor, double y2Coor)
        {
            double CalculatedDistance = Calculate.DistanceTo(x1Coor, x2Coor, y1Coor, y2Coor);

            Assert.That(CalculatedDistance, Is.EqualTo(1946.124353683494707425007744333489429246012287529387513188));
        }

        [Test]
        public void TestUpdate() 
        {
            DateTime dateTime1 = DateTime.ParseExact("20151006213456789", "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            Aircraft aircraft1 = new Aircraft("ATR423", 39045, 12932, 14000, dateTime1);

            DateTime dateTime2 = DateTime.ParseExact("20151006213558789", "yyyyMMddHHmmssfff", System.Globalization.CultureInfo.InvariantCulture);
            Aircraft aircraft2 = new Aircraft("ATR423", 39100, 13000, 15000, dateTime2);

            Calculate.Update(aircraft1, aircraft2);
            Assert.AreEqual(aircraft2.HorizontalVelocity, 1.4106219615539501);
        }
    }
}
