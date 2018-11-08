using ATMHandin3.Events;

namespace ATMHandin3.Interfaces
{
    public interface IFileLogger
    {
        void ReceiverWarningRaised(object sender, SeparationEventArgs e);

    }
}
