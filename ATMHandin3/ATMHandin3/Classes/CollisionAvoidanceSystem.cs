using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class CollisionAvoidanceSystem : ICollisionAvoidanceSystem
    {
        public event EventHandler<SeparationEventArgs> SeparationEvent;
        private IAMSController _eventReceiver;
        private List<Aircraft> aircraftsToSeparate;
        private List<string> warnings = new List<string>();
        private double _longitudeTolerance;
        private double _altitudeTolerance;
        
        public CollisionAvoidanceSystem(IAMSController eventReceiver, double longitudeTolerance, double altitudeTolerance)
        {
            _eventReceiver = eventReceiver;

            aircraftsToSeparate = new List<Aircraft>();

            eventReceiver.FilteredAircraftsEvent += ReceiverFilteredDataReady;

            _longitudeTolerance = longitudeTolerance;
            _altitudeTolerance = altitudeTolerance;

            // _aircraftsInAirspace = _amsController._aircraftsInsideAirspace;
        }

        public void ReceiverFilteredDataReady(object sender, AircraftsFilteredEventArgs e)
        {
           CollisionWarning(e.filteredAircraft);
        }

        public void CollisionWarning(IDictionary<string, Aircraft> aircrafts)
        {
            var newWarningsList = new List<string>();
            for (int i = 0; i < aircrafts.Count; i++)
            {
                for (int j = i+1; j < aircrafts.Count; j++)
                {
                    var ac1 = aircrafts.Values.ElementAt(i);
                    var ac2 = aircrafts.Values.ElementAt(j);

                    int diffAltitude = calculateAltitudeDiff(ac1.Altitude, ac2.Altitude);

                    double diffLongitude = distanceTo(
                        ac1.XCoordinate,
                        ac2.XCoordinate,
                        ac1.YCoordinate,
                        ac2.YCoordinate);

                    if (diffAltitude <= _altitudeTolerance && diffLongitude <= _longitudeTolerance)
                    {
                        FileStream fs = new FileStream(@"WriteLines.txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs);
                        TextWriter tw = Console.Out;

                        Console.SetOut(sw);
                        Console.WriteLine("WARNING!!!! {0}, you are on a collision course with {1}. At: {2}. Divert course!",
                            ac1.Tag, ac2.Tag, ac1.TimeStamp);
                        Console.SetOut(tw);

                        sw.Close();
                        fs.Close();

                        newWarningsList.Add(string.Format("WARNING!!!! {0}, you are on a collision course with {1}. At: {2}. Divert course!", ac1.Tag, ac2.Tag, ac1.TimeStamp));


                    }
                }
            }

            if (newWarningsList.Count == 0)
            {
                warnings = new List<string>() { "No current collision warnings" };
            }
            else
            {
                warnings = newWarningsList;
            }
        }

        private double distanceTo(double x1coor, double x2coor, double y1coor, double y2coor)
        {
            double xdiif = x1coor - x2coor;
            double ydiif = y1coor - y2coor;
            double longtitude = Math.Sqrt(Math.Pow(xdiif, 2) + Math.Pow(ydiif, 2));

            return longtitude;
        }

        private int calculateAltitudeDiff(int alti1, int alti2)
        {
            int diffAlti;

            if (alti1 > alti2)
            {
                diffAlti = alti1 - alti2;
                return diffAlti;
            }
            diffAlti = alti2 - alti1;
            return diffAlti;
        }

        public void PrintAllWarnings()
        {
            foreach (var war in warnings)
            {
                Console.WriteLine(war);
            }
        }
    }
}
