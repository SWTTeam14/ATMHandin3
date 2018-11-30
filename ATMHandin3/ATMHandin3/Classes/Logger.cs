using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class Logger : ILogger
    {
        private ICollisionAvoidanceSystem _eventReceiver;
        private IFileLogger _fileLogger;

        public Logger(ICollisionAvoidanceSystem eventReceiver)
        {
            _fileLogger = new FileLogger();
            _eventReceiver = eventReceiver;
            _eventReceiver.SeparationEvent += ReceiverWarningRaised;
            File.Delete(@"SeparationEventLogFile.txt");
        }

        public void ReceiverWarningRaised(object sender, SeparationEventArgs e)
        {
            string str = String.Format("Collision event; {0}; {1}; {2}", e.a1.Tag, e.a2.Tag, e.a1.TimeStamp);
            _fileLogger.WriteToFile(str);
        }


        public bool CheckIfFileExists()
        {
            return File.Exists(@"SeparationEventLogFile.txt");
        }
    }
}
