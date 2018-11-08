using System.IO;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class FileLogger : IFileLogger
    {
        private ICollisionAvoidanceSystem _eventReceiver;

        public FileLogger(ICollisionAvoidanceSystem eventReceiver)
        {
            _eventReceiver = eventReceiver;
            _eventReceiver.SeparationEvent += ReceiverWarningRaised;
        }

        public void ReceiverWarningRaised(object sender, SeparationEventArgs e)
        {
            StreamWriter log;

            if (!(File.Exists(@"SeparationEventLogFile")))
            {
                File.Create(@"SeparationEventLogFile");
            }

            if(File.ReadAllText(@"SeparationEventLogFile") != e.a1.Tag)
            {
                using (log = File.AppendText(@"SeparationEventLogFile.txt"))
                {
                    log.WriteLine("Collision event; {0}; {1}; {2}", e.a1.Tag, e.a2.Tag, e.a1.TimeStamp);
                }
            }
        }
    }
}
