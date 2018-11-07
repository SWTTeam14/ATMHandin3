using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ATMHandin3.Events;
using ATMHandin3.Interfaces;
using TransponderReceiver;

namespace ATMHandin3.Classes
{

    public class Decoder : IDecoder
    {
        public event EventHandler<DataDecodedEventArgs> DataDecodedEvent;

        private ITransponderReceiver Receiver;
        private List<Aircraft> aircrafts;

        public Decoder(ITransponderReceiver receiver)
        {
            
            Receiver = receiver;
            aircrafts = new List<Aircraft>();
            Receiver.TransponderDataReady += ReceiverTransponderDataReady;
        }

        public void ReceiverTransponderDataReady(object sender, RawTransponderDataEventArgs e)
        {

            aircrafts.Clear();

            foreach (var data in e.TransponderData)
            {
                
                aircrafts.Add(convertData(data));
                
            }
            DataDecodedEvent(this, new DataDecodedEventArgs(aircrafts));

            //onDataDecodedEvent(new DataDecodedEventArgs(aircrafts));
        }

        public void onDataDecodedEvent(DataDecodedEventArgs e)
        {
            DataDecodedEvent?.Invoke(this, e);
        }
        
        public Aircraft convertData(string data)
        {
            string[] tokens;
            char[] separators = { ';' };
            tokens = data.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            Aircraft aircraft = new Aircraft(tokens[0],int.Parse(tokens[1]), int.Parse(tokens[2]), int.Parse(tokens[3]), convertTime(tokens[4]));
            
            return aircraft;
        }

        public DateTime convertTime(string data)
        {

            DateTime myDate = DateTime.ParseExact(data, "yyyyMMddHHmmssfff",
                System.Globalization.CultureInfo.InvariantCulture);

            return myDate;
        }

    }


}
