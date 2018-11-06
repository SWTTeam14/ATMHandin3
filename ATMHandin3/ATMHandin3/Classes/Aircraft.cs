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


    }
}
