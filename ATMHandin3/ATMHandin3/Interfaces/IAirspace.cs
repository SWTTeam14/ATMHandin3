using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;

namespace ATMHandin3.Interfaces
{
    public interface IAirspace
    {
        int South { get; set; }
        int West { get; set; }
        int North { get; set; }
        int East { get; set; }
        int LowerAltitude { get; set; }
        int UpperAltitude { get; set; }
        
    }
}
