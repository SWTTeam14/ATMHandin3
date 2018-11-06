using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class AMSController : IAMSController
    {
        public event EventHandler<AircraftsFilteredEventArgs> FilteredAircraftsEvent;
        public event EventHandler<TrackEnteredAirspaceEventArgs> TrackEnteredAirspaceEvent;
        public event EventHandler<TrackLeftAirspaceEventArgs> TrackLeftAirspaceEvent;
        public event EventHandler<SeparationEventArgs> SeparationEvent;


        private IDecoder _decoder;
        public AMSController(IDecoder decoder)
        {
            _decoder = decoder;

            _decoder.DataDecodedEvent += DataDecodedEventHandler;
        }

        public void DataDecodedEventHandler(object sender, DataDecodedEventArgs e)
        {
            foreach (var Aircraft in e.Aircraft)
            {
                
            }
        }


    }
}
