using System;
using System.IO;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class FileLogger : IFileLogger
    {


        public void WriteToFile(string stringToBeWritten)
        {
            using (var log = File.AppendText(@"SeparationEventLogFile.txt"))
            {
                log.WriteLine(stringToBeWritten);
            }
        }
    }
}
