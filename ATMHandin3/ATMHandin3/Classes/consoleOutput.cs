using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;
using System.Timers;
namespace ATMHandin3.Classes
{
    public class consoleOutput
    {
        private static Mutex mut = new Mutex();
        private static Mutex mut1 = new Mutex();
        private static Mutex mut2 = new Mutex();

        private IAMSController _amscontroller;
        private ICollisionAvoidanceSystem _collisionAvoidanceSystem;
        private ITimer _timer;
        private int countthreads = 0;
        private List<Aircraft> aircraftsJustEnteredAirspace;
        private List<Aircraft> aircraftsJustExistedAirspace;

        private List<SeparationEventArgs> aircraftsColliding;

        public consoleOutput(IAMSController cont, ITimer timer, ICollisionAvoidanceSystem collision)
        {
            _timer = timer;
            _collisionAvoidanceSystem = collision;

            aircraftsColliding = new List<SeparationEventArgs>();

            aircraftsJustEnteredAirspace = new List<Aircraft>();
            aircraftsJustExistedAirspace = new List<Aircraft>();

            _amscontroller = cont;
            _amscontroller.TrackEnteredAirspaceEvent += trackEnteredAirspaceEventHandler;
            _amscontroller.TrackLeftAirspaceEvent += trackLeftAirspaceEventHandler;
            _amscontroller.FilteredAircraftsEvent += aircraftsInsideAirspaceEventHandler;

            _collisionAvoidanceSystem.SeparationEvent += collisionEventHandler;

        }



        public void collisionEventHandler(object sender, SeparationEventArgs e)
        {
            mut2.WaitOne();
            
            aircraftsColliding.Add(e);
            mut2.ReleaseMutex();
        }
        public void trackEnteredAirspaceEventHandler(object sender, TrackEnteredAirspaceEventArgs e)
        {

            Thread t1 = new Thread(new ThreadStart(() =>
            {
               
                aircraftsJustEnteredAirspace.Add(e.aircraft);
                Thread.Sleep(5000);
                aircraftsJustEnteredAirspace.RemoveAll(aircraft => e.aircraft == e.aircraft);
                
            }));

            t1.Start();
            
        }


        public void trackLeftAirspaceEventHandler(object sender, TrackLeftAirspaceEventArgs e)
        {
            Thread t1 = new Thread(new ThreadStart(() =>
            {

                aircraftsJustExistedAirspace.Add(e.aircraft);
                Thread.Sleep(5000);
                aircraftsJustExistedAirspace.RemoveAll(aircraft => e.aircraft == e.aircraft);


            }));

            t1.Start();
        }

        public void aircraftsInsideAirspaceEventHandler(object sender, AircraftsFilteredEventArgs e)
        {
            Console.Clear();
            foreach (var aircraft in e.filteredAircraft)
            {
                string dateTimeString = aircraft.Value.TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss fff");
                string str = string.Format("Tag:{0}\t X coordinate:{1} meters \t Y coordinate:{2} meters \tAltitude:{3} meters\t Timestamp:{4}\tCompassCourse:{5}\tHorizontalVelocity:{6}", 
                    aircraft.Value.Tag, aircraft.Value.XCoordinate, aircraft.Value.YCoordinate, aircraft.Value.Altitude, dateTimeString, aircraft.Value.CompassCourse, aircraft.Value.HorizontalVelocity);
                Console.WriteLine(str);
            }

            if (aircraftsJustEnteredAirspace.Count > 0)
            {
                mut.WaitOne();
                foreach (var aircraft in aircraftsJustEnteredAirspace)
                {
                    string dateTimeString = aircraft.TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss fff");
                    string str = string.Format("\nAIRCRAFT ENTERED AIRSPACE EVENT: Aircraft with tag:{0} just entered the airspace at time:{1}", aircraft.Tag,
                        dateTimeString);
                    Console.WriteLine(str);
                }
                mut.ReleaseMutex();
            }

            if (aircraftsJustExistedAirspace.Count > 0)
            {
                mut1.WaitOne();
                foreach (var aircraft in aircraftsJustExistedAirspace)
                {
                    string dateTimeString = aircraft.TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss fff");
                    string str = string.Format("\nAIRCRAFT EXITED AIRSPACE EVENT: Aircraft with tag:{0} just exited the airspace at time:{1}", aircraft.Tag,
                        dateTimeString);
                    Console.WriteLine(str);
                }
                mut1.ReleaseMutex();
            }


            if (aircraftsColliding.Count > 0)
            {
                mut2.WaitOne();
                try
                {
                    foreach (var aircraft in aircraftsColliding)
                    {
                        string dateTimeString = aircraft.a1.TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss fff");

                        string str = string.Format(
                            "\nAIRCRAFTS ARE COLLIDING: Aircraft with tag: {0} and {2} are colliding at time: {1}",
                            aircraft.a1.Tag,
                            dateTimeString, aircraft.a2.Tag);

                        Console.WriteLine(str);
                        

                    }
                }
                finally
                {
                    mut2.ReleaseMutex();
                }
                aircraftsColliding.Clear();


            }
            int count = e.filteredAircraft.Count;
            Console.WriteLine("Number of airplanes inside airspace : " + count);
            Console.WriteLine("Number of airplanes colliding: " + aircraftsColliding.Count);
        }



    }
}
