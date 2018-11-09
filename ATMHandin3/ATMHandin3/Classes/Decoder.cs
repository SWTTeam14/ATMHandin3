using System;
using System.Collections.Generic;
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
                aircrafts.Add(ConvertData(data));   
            }
            DataDecodedEvent?.Invoke(this, new DataDecodedEventArgs(aircrafts));
        }
        
        public Aircraft ConvertData(string data)
        {
            string[] tokens;
            char[] separators = { ';' };
            tokens = data.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            Aircraft aircraft = new Aircraft(tokens[0],int.Parse(tokens[1]), int.Parse(tokens[2]), int.Parse(tokens[3]), ConvertTime(tokens[4]));
            return aircraft;
        }

        public DateTime ConvertTime(string data)
        {
            DateTime myDate = DateTime.ParseExact(data, "yyyyMMddHHmmssfff",
                System.Globalization.CultureInfo.InvariantCulture);
            return myDate;
        }
    }
}
