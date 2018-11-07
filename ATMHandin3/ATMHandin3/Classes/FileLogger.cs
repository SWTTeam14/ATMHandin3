using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class FileLogger : IFileLogger
    {
        private ICollisionAvoidanceSystem eventReceiver;

        public FileLogger(ICollisionAvoidanceSystem _eventReceiver)
        {
            eventReceiver = _eventReceiver;

            _eventReceiver.SeparationEvent += ReceiverWarningRaised;
        }

        public void ReceiverWarningRaised(object sender, SeparationEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = i+1; j < 2; j++)
                {
                    var air1 = e.a1;
                    var air2 = e.a2;

                    //FileStream fs = new FileStream(@"SeparationEventLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
                    //StreamWriter sw = new StreamWriter(fs);
                    //TextWriter tw = Console.Out;
                    //
                    ////TextWriter tsw = new StreamWriter(@"SeparationEventLog.txt", true);
                    //
                    //Console.SetOut(sw);
                    ////Console.WriteLine("WARNING!!!! {0}, you are on a collision course with {1}. At: {2}. Divert course!",
                    ////    air1.Tag, air2.Tag, air1.TimeStamp, Environment.NewLine);
                    //
                    //
                    //sw.WriteLine("WARNING!!!! {0}, you are on a collision course with {1}. At: {2}. Divert course!",
                    //    air1.Tag, air2.Tag, air1.TimeStamp);
                    //Console.SetOut(tw);
                    //
                    //sw.Close();
                    //fs.Close();

                    StreamWriter log;
                    string writeToLog =
                        string.Format("WARNING!!!! {0}, you are on a collision course with {1}. At: {2}. Divert course!",
                        air1.Tag, air2.Tag, air1.TimeStamp);
                    
                    string textInFile = File.ReadAllText(@"SeparationEventLogFile.txt");

                    if (!textInFile.Contains(air1.Tag))
                    {
                        
                        if (!File.Exists(@"SeparationEventLogFile.txt"))
                        {
                            log = new StreamWriter(@"SeparationEventLogFile.txt");
                        }
                        else
                        {
                            log = File.AppendText(@"SeparationEventLogFile.txt");
                        }

                        log.WriteLine(writeToLog);
                        log.Close();
                    }
                }
            }

            
        }
        
    }
}
