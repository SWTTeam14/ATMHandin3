using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;

namespace ATMHandin3.Interfaces
{
    public interface ICalculate
    {
        double CalculateVelocity(int xcoor1, int xcoor2, int ycoor1, int ycoor2, DateTime timestamp1, DateTime timestamp2);
        double CalculateAngle(double xcoor1, double xcoor2, double ycoor1, double ycoor2);
        void Update(Aircraft aircraft, Aircraft updatedAircraft);
    }
}
