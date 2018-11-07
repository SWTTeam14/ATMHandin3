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

        private IAMSController _amscontroller;
        private ICollisionAvoidanceSystem _collisionAvoidanceSystem;
        private ITimer _timer;
        private int countthreads = 0;
        private List<Aircraft> aircraftsJustEnteredAirspace;
        private List<Aircraft> aircraftsJustExistedAirspace;

        private List<Aircraft> aircraftsColliding;

        public consoleOutput(IAMSController cont, ITimer timer, ICollisionAvoidanceSystem collision)
        {
            _timer = timer;
            _collisionAvoidanceSystem = collision;

            aircraftsColliding = new List<Aircraft>();

            aircraftsJustEnteredAirspace = new List<Aircraft>();
            aircraftsJustExistedAirspace = new List<Aircraft>();

            _amscontroller = cont;
            _amscontroller.TrackEnteredAirspaceEvent += trackEnteredAirspaceEventHandler;
            _amscontroller.TrackLeftAirspaceEvent += trackLeftAirspaceEventHandler;
            _amscontroller.FilteredAircraftsEvent += aircraftsInsideAirspaceEventHandler;

            _collisionAvoidanceSystem.SeparationEvent += collisionEventHandler;
            _collisionAvoidanceSystem.noMoreSeperationEvent += noCollisionEventHandler;

        }

        public void noCollisionEventHandler(object sender, noMoreSeperationEventArgs e)
        {
            aircraftsColliding.Remove(e.a1);
            aircraftsColliding.Remove(e.a2);
        }

        public void collisionEventHandler(object sender, SeparationEventArgs e)
        {
            //Thread t1 = new Thread(new ThreadStart(() =>
            //{
            //    if (aircraftsColliding.Count == 0)
            //    {
            //        aircraftsColliding.Add(e.a1);
            //        aircraftsColliding.Add(e.a2);
            //    }

            //    for (int k = 0; k < aircraftsColliding.Count; k++)
            //    {
            //        if (!(aircraftsColliding[k].Tag == e.a1.Tag || aircraftsColliding[k].Tag == e.a2.Tag))
            //        {
            //            aircraftsColliding.Add(e.a1);
            //            aircraftsColliding.Add(e.a2);
            //        }
            //    }
            //    //Thread.Sleep(5000);
                
            //}));

            //t1.Start();
            aircraftsColliding.Add(e.a1);
            aircraftsColliding.Add(e.a2);
            
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
                mut1.WaitOne();
                foreach (var aircraft in aircraftsColliding)
                {
  
                    Console.WriteLine($"WARNING! Possible collision between flight {aircraft} " +
                                      $"and {aircraft.SecondAircraft.Tag}. {aircraft.FirstAircraft.TimeStamp}");
                }



                for (int i = 0; i < aircraftsColliding.Count; i+=2)
                {
                    string dateTimeString = aircraftsColliding[i].TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss fff");

                    string str = string.Format("\nAIRCRAFTS ARE COLLIDING: Aircraft with tag: {0} and {2} are colliding at time: {1}", aircraftsColliding[i].Tag,
                        dateTimeString, aircraftsColliding[i+1].Tag);

                    Console.WriteLine(str);
                    aircraftsColliding.Clear();
                }
                mut1.ReleaseMutex();
            }
            int count = e.filteredAircraft.Count;
            Console.WriteLine("Number of airplanes inside airspace : " + count);
            Console.WriteLine("Number of airplanes colliding: " + aircraftsColliding.Count);
        }



    }
}
