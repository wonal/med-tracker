using medtracker.Config;
using System;

namespace medtracker
{
    public class Utilities
    {
        public static int CalculateSeconds (DateTime time)
        {
            return (60 * 60 * time.Hour) + (time.Minute*60) + time.Second;
        }

        public static DateTime NextReportDate(DateTime time)
        {
            return new DateTime(time.Year, time.Month, Constants.reportDay, 8, 0, 0);
        }

        
    }
}
