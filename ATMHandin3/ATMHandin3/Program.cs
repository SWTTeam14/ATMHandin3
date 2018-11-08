using System.Threading;
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
            AMSController ams = new AMSController(d1, new Airspace(
            
                South: 10000,
                West: 10000,
                North: 90000,
                East: 90000,
                LowerAltitude: 500,
                UpperAltitude: 20000
            ));

            CollisionAvoidanceSystem cas = new CollisionAvoidanceSystem(ams,50000,10000);
            FileLogger fl = new FileLogger(cas);

            ConsoleOutput c = new ConsoleOutput(ams,cas);

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
