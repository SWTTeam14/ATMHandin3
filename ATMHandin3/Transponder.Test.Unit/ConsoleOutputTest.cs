using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ATMHandin3.Classes;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;
using NUnit.Framework;
using NSubstitute;

namespace Transponder.Test.Unit
{
    [TestFixture]
    public class ConsoleOutputTest
    {
        private ConsoleOutput _uut;
        private IOutput output;
        private IAMSController amsController;
        private ICollisionAvoidanceSystem collisionAvoidanceSystem;

        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            amsController = Substitute.For<IAMSController>();
            collisionAvoidanceSystem = Substitute.For<ICollisionAvoidanceSystem>();
            _uut = new ConsoleOutput(amsController, collisionAvoidanceSystem, output);
        }

        [Test]
        public void SeparationEvent_Correct_output_with_tags()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 9000, 5000, new DateTime(1994, 09, 3));

            collisionAvoidanceSystem.SeparationEvent += Raise.EventWith(this, new SeparationEventArgs(a1, a2));
           
            _uut.OutputAircraftsColliding();
            output.Received().OutputWriteline(Arg.Is<string>(str=>str.Contains("AIRCRAFTS ARE COLLIDING: Aircraft with tag: ttt and yyy are colliding at time: ")));
        }

        [Test]
        public void SeparationEvent_correct_number_of_airplanes_in_colliding_list()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 9000, 5000, new DateTime(1994, 09, 3));

            collisionAvoidanceSystem.SeparationEvent += Raise.EventWith(this, new SeparationEventArgs(a1, a2));
            collisionAvoidanceSystem.SeparationEvent += Raise.EventWith(this, new SeparationEventArgs(a1, a2));

            Assert.That(2, Is.EqualTo(_uut.AircraftsColliding.Count));
        }

        [Test]
        public void Aircraft_inside_airspace_event_correct_insert_and_remove_from_list_TESTING_THREAD()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 9000, 5000, new DateTime(1994, 09, 3));

            amsController.TrackEnteredAirspaceEvent +=
                Raise.EventWith(this, new TrackEnteredAirspaceEventArgs(a1));

            amsController.TrackEnteredAirspaceEvent +=
                Raise.EventWith(this, new TrackEnteredAirspaceEventArgs(a2));

            Thread.Sleep(10);
            Assert.That(2, Is.EqualTo(_uut.AircraftsJustEnteredAirspace.Count));

            Thread.Sleep(7000);
            Assert.AreEqual(0, _uut.AircraftsJustEnteredAirspace.Count);
        }

        [Test]
        public void Aircraft_inside_airspace_event_correct_insert_and_no_remove_from_list_because_less_than_5_seconds_pass()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 9000, 5000, new DateTime(1994, 09, 3));


            amsController.TrackEnteredAirspaceEvent +=
                Raise.EventWith(this, new TrackEnteredAirspaceEventArgs(a1));

            amsController.TrackEnteredAirspaceEvent +=
                Raise.EventWith(this, new TrackEnteredAirspaceEventArgs(a2));

            Thread.Sleep(1000);
            Assert.That(2, Is.EqualTo(_uut.AircraftsJustEnteredAirspace.Count));

            Thread.Sleep(3000);
            Assert.That(2, Is.EqualTo(_uut.AircraftsJustEnteredAirspace.Count));

        }

        [Test]
        public void Aircraft_just_exited_airspace_event_correct_insert_and_remove_from_list()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 9000, 5000, new DateTime(1994, 09, 3));


            amsController.TrackLeftAirspaceEvent +=
                Raise.EventWith(this, new TrackLeftAirspaceEventArgs(a1));

            amsController.TrackLeftAirspaceEvent +=
                Raise.EventWith(this, new TrackLeftAirspaceEventArgs(a2));

            Thread.Sleep(1000);
            Assert.AreEqual(2, _uut.AircraftsJustExcitedAirspace.Count);

            Thread.Sleep(6000);
            Assert.AreEqual(0, _uut.AircraftsJustExcitedAirspace.Count);
        }

        [Test]
        public void Aircraft_exited_airspace_event_correct_insert_and_no_remove_from_list_because_less_than_5_seconds_pass()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 9000, 5000, new DateTime(1994, 09, 3));


            amsController.TrackLeftAirspaceEvent +=
                Raise.EventWith(this, new TrackLeftAirspaceEventArgs(a1));

            amsController.TrackLeftAirspaceEvent +=
                Raise.EventWith(this, new TrackLeftAirspaceEventArgs(a2));

            Thread.Sleep(10);
            Assert.That(2, Is.EqualTo(_uut.AircraftsJustExcitedAirspace.Count));

            Thread.Sleep(4000);
            Assert.That(2, Is.EqualTo(_uut.AircraftsJustExcitedAirspace.Count));
        }

        [Test]
        public void Aircraft_exited_airspace_correct_tag_output()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            amsController.TrackLeftAirspaceEvent += Raise.EventWith(this, new TrackLeftAirspaceEventArgs(a1));

            // This sleep is added because the eventhandler needs time to be called after the event is raised.
            Thread.Sleep(1000);

            _uut.OutputAircraftsWhoJustExitedAirspace();
           
            output.Received().OutputWriteline(Arg.Is<string>(str => 
                str.Contains("AIRCRAFT EXITED AIRSPACE EVENT: Aircraft with tag:ttt just exited the airspace at time:")));
         }

        [Test]
        public void Aircraft_inside_airspace_output_method_correct()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Dictionary<string, Aircraft> aircrafts = new Dictionary<string, Aircraft>();

            aircrafts.Add("ttt", a1);
            aircrafts.Add("yyy", a2);

           _uut.OutputAircraftsInsideAirspace(new AircraftsFilteredEventArgs(aircrafts));

            output.Received().OutputWriteline(Arg.Is<string>(str => 
                str.Contains("Tag:ttt\t X coordinate:9000 meters \t Y coordinate:9000 meters \tAltitude:5000 meters\t Timestamp:")));
        }

        [Test]
        public void Aircraft_inside_airspace_event_handler_output_correct()
        {
            Aircraft a1 = new Aircraft("ttt", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Aircraft a2 = new Aircraft("yyy", 9000, 9000, 5000, new DateTime(1994, 09, 3));
            Dictionary<string, Aircraft> aircrafts = new Dictionary<string, Aircraft>();

            aircrafts.Add("ttt", a1);
            aircrafts.Add("yyy", a2);

            amsController.FilteredAircraftsEvent += Raise.EventWith(amsController, new AircraftsFilteredEventArgs(aircrafts));

            output.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("9000")));
            output.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("ttt")));
            output.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("yyy")));
            output.Received().OutputWriteline(Arg.Is<string>(str => str.Contains("5000")));
        }
    }
}
