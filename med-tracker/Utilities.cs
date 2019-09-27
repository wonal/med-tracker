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
            return new DateTime(time.Year, time.Month, time.Day, 8, 0, 0);
        }

        //Reports are for the current month unless requested on the 1st day, in which the report is for the entire previous month
        public static string FormattedReportMonth(DateTime time)
        {
            if (time.Day == 1)
            {
                if (time.Month == 1) time.AddMonths(-1);
                return time.ToString("MMMM");
            }
            return time.ToString("MMMM");
        }
    }
}
