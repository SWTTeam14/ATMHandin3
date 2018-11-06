using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATMHandin3.Classes;
using TransponderReceiver;
using Decoder = ATMHandin3.Classes.Decoder;

namespace ATMHandin3
{
    class Program
    {
        static void Main(string[] args)
        {

            ITransponderReceiver receiver = TransponderReceiverFactory.CreateTransponderDataReceiver();

            Decoder d1 = new Decoder(receiver);
            AMSController ams = new AMSController(d1,new Airspace());


            while (true)
            {
                Thread.Sleep(1000);
                Console.Clear();
                d1.print();
                //cas.Seperate();
            }
        }
    }
}
