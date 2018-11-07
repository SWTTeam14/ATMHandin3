using System;

namespace ATMHandin3.Classes
{
    public class Aircraft
    {
        public Aircraft(string tag, int xCoordinate, int yCoordinate, int altitude, DateTime timestamp)
        {
            Tag = tag;
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            Altitude = altitude;
            TimeStamp = timestamp;
        }

        public string Tag { get; set; }
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int Altitude { get; set; }
        public DateTime TimeStamp { get; set; }
        public int CompassCourse { get; set; }
        public int HorizontalVelocity { get; set; }

        //public override string ToString()
        //{
        //    string dateTimeString = TimeStamp.ToString("MMMM dd, yyyy HH:mm:ss fff");
        //    return string.Format("Tag:\t\t\t{0}\nX coordinate:\t\t{1} meters\nY coordinate:\t\t{2} meters\nAltitude:\t\t{3} meters\nTimestamp:\t\t{4}\nCompassCourse:\t\t{5}\nHorizontalVelocity:\t{6}\n", Tag, XCoordinate, YCoordinate, Altitude, dateTimeString, CompassCourse, HorizontalVelocity);
        //}

    }
}
