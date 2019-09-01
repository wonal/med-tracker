using System;

namespace medtracker
{
    public class Utilities
    {
        public static int CalculateMinutes (DateTime time)
        {
            return (60 * time.Hour) + time.Minute;
        }
    }
}
