using System;
using System.Threading;
using ATMHandin3.Classes;
using TransponderReceiver;
using Decoder = ATMHandin3.Classes.Decoder;
using ATMHandin3.Interfaces;
using Timer = ATMHandin3.Classes.Timer;

namespace ATMHandin3
{
    class Program
    {
        static void Main(string[] args)
        {

            ITransponderReceiver receiver = TransponderReceiverFactory.CreateTransponderDataReceiver();

            Decoder d1 = new Decoder(receiver);
            AMSController ams = new AMSController(d1, new Airspace(
            
                South: 10000,
                West: 10000,
                North: 90000,
                East: 90000,
                LowerAltitude: 500,
                UpperAltitude: 20000
            ));

            CollisionAvoidanceSystem cas = new CollisionAvoidanceSystem(ams,70000,10000);
            FileLogger fl = new FileLogger(cas);

            Timer timer = new Timer();
            consoleOutput c = new consoleOutput(ams, timer,cas);

            while (true)
            {
                Thread.Sleep(1000);
                
                //ams.Print();
                //cas.PrintAllWarnings();
            }
        }
    }
}
