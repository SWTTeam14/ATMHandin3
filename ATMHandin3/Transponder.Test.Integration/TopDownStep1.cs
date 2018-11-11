using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATMHandin3.Classes;
using ATMHandin3.Interfaces;
using NUnit.Framework;
using NSubstitute;
using TransponderReceiver;
using ATMHandin3.Classes;

namespace Transponder.Test.Integration
{
    [TestFixture]
    public class TopDownStep1
    {
        private ITransponderReceiver transponder;
        private IAirspace airspace;
        private ICollisionAvoidanceSystem iCol;
        private ATMHandin3.Classes.Decoder decoder;

        private AMSController amsController;


        private int _nFilteredAircraftEvent = 0;
        private int _nTrackEnteredAirspaceEvent = 0;
        private int _nTrackLeftAirspaceEvent = 0;

        [SetUp]
        public void SetUp()
        {
            airspace = new Airspace(10000, 10000, 90000, 90000, 500, 20000);

            amsController = new AMSController(decoder, airspace);
            decoder = new Decoder(transponder);


            iCol = Substitute.For<ICollisionAvoidanceSystem>();
            transponder = Substitute.For<ITransponderReceiver>();


            amsController.FilteredAircraftsEvent += (o, args) => { ++_nFilteredAircraftEvent; };
            amsController.TrackEnteredAirspaceEvent += (o, args) => { ++_nTrackEnteredAirspaceEvent; };
            amsController.TrackLeftAirspaceEvent += (o, args) => { ++_nTrackLeftAirspaceEvent; };
        }

        [Test]
        public void Test_seperation_event_correct_count()
        {
            string testData1 = "ATR423;39045;12932;14000;20151006213456789";
            string testData2 = "GFD123;39045;12932;14000;20151006213456789";
            string testData3 = "MKD936;39045;12932;14000;20151006213456789";

            List<string>
            transponder.TransponderDataReady += Raise.EventWith(this,);

        }
}
}
