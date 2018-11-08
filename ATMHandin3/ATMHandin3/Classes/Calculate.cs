using System;
using ATMHandin3.Interfaces;

namespace ATMHandin3.Classes
{
    public static class Calculate
    {
        public static double CalculateVelocity(double xcoor1, double xcoor2, double ycoor1, double ycoor2, DateTime timestamp1, DateTime timestamp2)
        {
            double xdiff = xcoor2 - xcoor1;
            double ydiff = ycoor2 - ycoor1;

            double distance = Math.Sqrt(Math.Pow(xdiff, 2) + Math.Pow(ydiff, 2));

            double velocity = (distance / (timestamp2 - timestamp1).TotalSeconds);

            return velocity;
        }

        public static double CalculateAngle(double xcoor1, double xcoor2, double ycoor1, double ycoor2)
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

        public static double DistanceTo(double x1coor, double x2coor, double y1coor, double y2coor)
        {
            double xdiif = x1coor - x2coor;
            double ydiif = y1coor - y2coor;
            double longtitude = Math.Sqrt(Math.Pow(xdiif, 2) + Math.Pow(ydiif, 2));

            return longtitude;
        }

        public static double CalculateAltitudeDiff(double alti1, double alti2)
        {
            double diffAlti;

            if (alti1 > alti2)
            {
                diffAlti = alti1 - alti2;
                return diffAlti;
            }
            diffAlti = alti2 - alti1;
            return diffAlti;
        }

        public static void Update(Aircraft a1, Aircraft a2)
        {
            //Calculating -> be aware that a2 would be modified
            a2.HorizontalVelocity = CalculateVelocity(a1.XCoordinate, a2.XCoordinate, a1.YCoordinate, a2.YCoordinate, a1.TimeStamp, a2.TimeStamp);
            a2.CompassCourse = CalculateAngle(a1.XCoordinate, a2.XCoordinate, a1.YCoordinate, a2.YCoordinate);
        }
    }
}
