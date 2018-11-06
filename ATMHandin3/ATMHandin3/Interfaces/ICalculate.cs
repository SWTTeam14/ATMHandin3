using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMHandin3.Interfaces
{
    public interface ICalculate
    {
        double CalculateVelocity();
        double CalculateAngle();
        void Update();
    }
}
