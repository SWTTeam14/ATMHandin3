using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class ConsoleOutput
    {
        private static Mutex mut = new Mutex();
        private static Mutex mut1 = new Mutex();
        private static Mutex mut2 = new Mutex();

        private IAMSController _amscontroller;
        private ICollisionAvoidanceSystem _collisionAvoidanceSystem;
        private ITimer _timer;
        private IOutput _output;
        
        private List<Aircraft> aircraftsJustEnteredAirspace;
        private List<Aircraft> aircraftsJustExistedAirspace;
        private List<SeparationEventArgs> aircraftsColliding;

        public ConsoleOutput(IAMSController amsController, ITimer timer, ICollisionAvoidanceSystem collision)
        {
            _output = new Output();
            _timer = timer;
            _collisionAvoidanceSystem = collision;
            aircraftsColliding = new List<SeparationEventArgs>();
            aircraftsJustEnteredAirspace = new List<Aircraft>();
            aircraftsJustExistedAirspace = new List<Aircraft>();
            _amscontroller = amsController;
            _amscontroller.TrackEnteredAirspaceEvent += TrackEnteredAirspaceEventHandler;
            _amscontroller.TrackLeftAirspaceEvent += TrackLeftAirspaceEventHandler;
            _amscontroller.FilteredAircraftsEvent += AircraftsInsideAirspaceEventHandler;
            _collisionAvoidanceSystem.SeparationEvent += CollisionEventHandler;
        }

        public void CollisionEventHandler(object sender, SeparationEventArgs e)
        {
            mut2.WaitOne();
            aircraftsColliding.Add(e);
            mut2.ReleaseMutex();
        }
        public void TrackEnteredAirspaceEventHandler(object sender, TrackEnteredAirspaceEventArgs e)
        {
            Thread t1 = new Thread(new ThreadStart(() =>
            {
                aircraftsJustEnteredAirspace.Add(e.aircraft);
                Thread.Sleep(5000);
                aircraftsJustEnteredAirspace.RemoveAll(aircraft => e.aircraft == e.aircraft);  
            }));

            t1.Start();
        }

        public void TrackLeftAirspaceEventHandler(object sender, TrackLeftAirspaceEventArgs e)
        {
            Thread t1 = new Thread(new ThreadStart(() =>
            {
                aircraftsJustExistedAirspace.Add(e.aircraft);
                Thread.Sleep(5000);
                aircraftsJustExistedAirspace.RemoveAll(aircraft => e.aircraft == e.aircraft);
            }));

            t1.Start();
        }

        public void AircraftsInsideAirspaceEventHandler(object sender, AircraftsFilteredEventArgs e)
        {
            Console.Clear();
            foreach (var aircraft in e.filteredAircraft)
            {
                string dateTimeString = aircraft.Value.TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss fff");
                string str = string.Format("Tag:{0}\t X coordinate:{1} meters \t Y coordinate:{2} meters \tAltitude:{3} meters\t Timestamp:{4}\tCompassCourse:{5}\tHorizontalVelocity:{6}", 
                    aircraft.Value.Tag, aircraft.Value.XCoordinate, aircraft.Value.YCoordinate, aircraft.Value.Altitude, dateTimeString, aircraft.Value.CompassCourse, aircraft.Value.HorizontalVelocity);
                
                _output.OutputWriteline(str);

            }
            _output.OutputWriteline("");

            if (aircraftsJustEnteredAirspace.Count > 0)
            {
                mut.WaitOne();
                foreach (var aircraft in aircraftsJustEnteredAirspace)
                {
                    string dateTimeString = aircraft.TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss fff");
                    string str = string.Format("AIRCRAFT ENTERED AIRSPACE EVENT: Aircraft with tag:{0} just entered the airspace at time:{1}", aircraft.Tag,
                        dateTimeString);
                    _output.OutputWriteline(str);
                    
                }
                _output.OutputWriteline("");
                mut.ReleaseMutex();
            }
            
            if (aircraftsJustExistedAirspace.Count > 0)
            {
                mut1.WaitOne();
                foreach (var aircraft in aircraftsJustExistedAirspace)
                {
                    string dateTimeString = aircraft.TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss fff");
                    string str = string.Format("AIRCRAFT EXITED AIRSPACE EVENT: Aircraft with tag:{0} just exited the airspace at time:{1}", aircraft.Tag,
                        dateTimeString);
                    _output.OutputWriteline(str);
                    _output.OutputWriteline("");
                }
                mut1.ReleaseMutex();
            }

            
            if (aircraftsColliding.Count > 0)
            {
                mut2.WaitOne();
                
                    foreach (var aircraft in aircraftsColliding)
                    {
                        string dateTimeString = aircraft.a1.TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss fff");

                        string str = string.Format(
                            "\nAIRCRAFTS ARE COLLIDING: Aircraft with tag: {0} and {2} are colliding at time: {1}",
                            aircraft.a1.Tag,
                            dateTimeString, aircraft.a2.Tag);

                    _output.OutputWriteline(str);
                    _output.OutputWriteline("");
                }
                
                
                    mut2.ReleaseMutex();
                
                aircraftsColliding.Clear();
            }
            int count = e.filteredAircraft.Count;

            _output.OutputWriteline("Number of airplanes inside airspace : " + count);
            
        }
    }
}
