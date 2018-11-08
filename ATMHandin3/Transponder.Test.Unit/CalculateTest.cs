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
        private DateTime tmpTime1;
        private DateTime tmpTime2;

        [SetUp]

        public void SetUp()
        {

        }

        [TestCase(14000, 12000, 57000, 52000)]
        public void TestCalculateVelocity(int a, int b, int c, int d)
        {
            tmpTime1 = new DateTime(2018, 07, 09, 20, 40, 10, 902);
            tmpTime2 = new DateTime(2018, 07, 09, 20, 40, 20, 902);

            //Assert.That(_uut.CalculateVelocity(a, b, c, d, tmpTime1, tmpTime2), Is.EqualTo(538.0));

        }
    }
}
