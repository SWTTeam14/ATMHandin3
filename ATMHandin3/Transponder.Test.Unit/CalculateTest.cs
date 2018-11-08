using System;
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
        public void TestCalculateVelocity(int a, int b, int c, int d)
        {
            _tmpTime1 = new DateTime(2018, 07, 09, 20, 40, 10, 902);
            _tmpTime2 = new DateTime(2018, 07, 09, 20, 40, 20, 902);

            Assert.That(Calculate.CalculateVelocity(a, b, c, d, _tmpTime1, _tmpTime2), Is.EqualTo(538.0));
        }

        [TestCase(0, 2, 0, 1, 63.4349488)]
        [TestCase(0, 2, 0, -1, 180 - 63.4349488)]
        [TestCase(0, -2, 0, -1, 180 + 63.4349488)]
        [TestCase(0, -2, 0, 1, (180 - 63.4349488) + 180)]
        public void TestCalculateAngle(double x1, double x2, double y1, double y2, double angle)
        {
            Assert.That(Calculate.CalculateAngle(x1, x2, y1, y2), Is.InRange(angle - 0.1, angle + 0.1));
        }
    }
}
