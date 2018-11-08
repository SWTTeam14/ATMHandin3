using System.IO;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class FileLogger
    {
        private ICollisionAvoidanceSystem _eventReceiver;

        public FileLogger(ICollisionAvoidanceSystem eventReceiver)
        {
            _eventReceiver = eventReceiver;
            _eventReceiver.SeparationEvent += ReceiverWarningRaised;
        }

        public void ReceiverWarningRaised(object sender, SeparationEventArgs e)
        {
            CheckIfFileExists();
            
            if (!File.ReadAllText(@"SeparationEventLogFile.txt").Contains(e.a1.Tag))
            { 
               using (var log = File.AppendText(@"SeparationEventLogFile.txt"))
               {
                   log.WriteLine("Collision event; {0}; {1}; {2}", e.a1.Tag, e.a2.Tag, e.a1.TimeStamp);
               }
            }
        }

        public bool CheckIfFileExists()
        {
            if (!(File.Exists(@"SeparationEventLogFile.txt")))
            {
                var separationFile = File.Create(@"SeparationEventLogFile.txt");
                separationFile.Close();
                return false;
            }

            return true;
        }
    }
}
