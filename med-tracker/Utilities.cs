using System;

namespace medtracker
{
    public class Utilities
    {
        public static int CalculateSeconds (DateTime time)
        {
            return (60 * 60 * time.Hour) + (time.Minute*60) + time.Second;
        }
    }
}
