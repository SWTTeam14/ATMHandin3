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
    }
}
