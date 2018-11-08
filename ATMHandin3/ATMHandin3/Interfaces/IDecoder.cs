using System;
using ATMHandin3.Events;

namespace ATMHandin3.Interfaces
{
    public interface IDecoder
    {
        event EventHandler<DataDecodedEventArgs> DataDecodedEvent;
    }
}
