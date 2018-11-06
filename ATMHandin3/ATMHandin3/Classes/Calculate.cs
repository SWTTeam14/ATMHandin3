using System;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public class Calculate : ICalculate
    {
        public double CalculateVelocity(int xcoor1, int xcoor2, int ycoor1, int ycoor2, DateTime timestamp1, DateTime timestamp2)
        {
            double xdiff = xcoor2 - xcoor1;
            double ydiff = ycoor2 - ycoor1;

            double distance = Math.Sqrt(Math.Pow(xdiff, 2) + Math.Pow(ydiff, 2));

            double velocity = (int)(distance / (timestamp2 - timestamp1).TotalSeconds);

            return velocity;
        }

        public double CalculateAngle(double xcoor1, double xcoor2, double ycoor1, double ycoor2)
        {
            double xdiff = xcoor2 - xcoor1;
            double ydiff = ycoor2 - ycoor1;

            double angle = Math.Atan(xdiff / ydiff) * 180 / Math.PI;

            if (ydiff < 0)
            {
                angle += 180;
            }
            else if (xdiff < 0 && ydiff > 0)
            {
                angle += 360;
            }
            return angle;
        }

        public void Update(Aircraft aircraft, Aircraft updatedAircraft)
        {
            //Calculating velocity
            updatedAircraft.HorizontalVelocity = (int)CalculateVelocity(updatedAircraft.XCoordinate, aircraft.XCoordinate, updatedAircraft.YCoordinate, aircraft.YCoordinate, updatedAircraft.TimeStamp, aircraft.TimeStamp);
            updatedAircraft.CompassCourse = (int)CalculateAngle(updatedAircraft.XCoordinate, aircraft.XCoordinate, updatedAircraft.YCoordinate, aircraft.YCoordinate);
            updatedAircraft.XCoordinate = aircraft.XCoordinate;
            updatedAircraft.YCoordinate = aircraft.YCoordinate;
            updatedAircraft.Altitude = aircraft.Altitude;
            updatedAircraft.TimeStamp = aircraft.TimeStamp;
        }
    }
}
