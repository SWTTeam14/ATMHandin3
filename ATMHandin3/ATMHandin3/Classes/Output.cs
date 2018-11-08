using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class Output : IOutput
    {
        public void OutputWriteline(string line)
        {
            Console.WriteLine(line);
        }

        public void ClearScreen()
        {
            Console.Clear();
        }
    }
}
