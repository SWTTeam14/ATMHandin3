using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Classes;
using ATMHandin3.Events;
using TransponderReceiver;

namespace ATMHandin3.Interfaces
{
    public interface IDecoder
    {
        void ReceiverTransponderDataReady(object sender, RawTransponderDataEventArgs e);
        void onDataDecodedEvent(DataDecodedEventArgs e);
        Aircraft convertData(string data);
        DateTime convertTime(string data);

        event EventHandler<DataDecodedEventArgs> DataDecodedEvent;
    }
}
