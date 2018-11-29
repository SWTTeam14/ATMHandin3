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
            File.Delete(@"SeparationEventLogFile.txt");
        }

        public void ReceiverWarningRaised(object sender, SeparationEventArgs e)
        {
            
            using (var log = File.AppendText(@"SeparationEventLogFile.txt"))
            {
                 log.WriteLine("Collision event; {0}; {1}; {2}", e.a1.Tag, e.a2.Tag, e.a1.TimeStamp);
            }
        }

        public bool CheckIfFileExists()
        {
            return File.Exists(@"SeparationEventLogFile.txt");
        }
    }
}
