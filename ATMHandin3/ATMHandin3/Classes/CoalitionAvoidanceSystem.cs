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
    public class CoalitionAvoidanceSystem : ICoalitionAvoidanceSystem
    {
        public event EventHandler<SeparationEventArgs> SeparationEvent;

        private IAMSController eventReceiver;
        
        private Aircraft _tmpAircraft;
        private Aircraft _tmpAircraftToCompare;

        private List<Aircraft> aircraftsToSeparate;

        public CoalitionAvoidanceSystem(IAMSController _eventReceiver)
        {
            eventReceiver = _eventReceiver;

            aircraftsToSeparate = new List<Aircraft>();

            eventReceiver.FilteredAircraftsEvent += ReceiverFilteredDataReady;

            // _aircraftsInAirspace = _amsController._aircraftsInsideAirspace;
        }

        public void ReceiverFilteredDataReady(object sender, AircraftsFilteredEventArgs e)
        {
            
                aircraftsToSeparate = CoalitionWarning(e.filteredAircraft);
            
        }

        public List<Aircraft> CoalitionWarning(List<Aircraft> aircrafts)
        {
            for (int i = 0; i < aircrafts.Count; i++)
            {
                for (int j = i + 1; j < aircrafts.Count; j++)
                {
                    int diffAltitude = calculateAltitudeDiff(aircrafts.ElementAt(i).Altitude,
                        aircrafts.ElementAt(j).Altitude);

                    double diffLongtitude = distanceTo(
                        aircrafts.ElementAt(i).XCoordinate,
                        aircrafts.ElementAt(j).XCoordinate,
                        aircrafts.ElementAt(i).YCoordinate,
                        aircrafts.ElementAt(j).YCoordinate);

                    if (diffAltitude <= 300 && diffLongtitude <= 5000)
                    {
                        _tmpAircraft = aircrafts.ElementAt(i);
                        _tmpAircraftToCompare = aircrafts.ElementAt(j);

                        FileStream fs = new FileStream(@"WriteLines.txt", FileMode.OpenOrCreate, FileAccess.Write);
                        StreamWriter sw = new StreamWriter(fs);
                        TextWriter tw = Console.Out;

                        Console.SetOut(sw);
                        Console.WriteLine("WARNING!!!! {0}, you are on a coalition course with {1}. At: {2}. Divert course!",
                            aircrafts.ElementAt(i).Tag, aircrafts.ElementAt(j).Tag,
                            aircrafts.ElementAt(i).TimeStamp);
                        Console.SetOut(tw);

                        sw.Close();
                        fs.Close();

                        Console.WriteLine("WARNING!!!! {0}, you are on a coalition course with {1}. At: {2}. Divert course!",
                            aircrafts.ElementAt(i).Tag, aircrafts.ElementAt(j).Tag,
                            aircrafts.ElementAt(i).TimeStamp);
                        
                        return aircrafts;
                    }
                }
            }
            Console.WriteLine("No current coalition warnings");
            return null;
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
    }
}
