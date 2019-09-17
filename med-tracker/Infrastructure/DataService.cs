using medtracker.DTOs;
using medtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace medtracker.Infrastructure
{
    public class DataService
    {
        public static MonthStats CalculateMonthlyStats(IEnumerable<DataDTO> monthlyData)
        {
            int totalHA = monthlyData.Where(x => x.ha_present == true).Count();
            int totalMaxalt = monthlyData.Select(x => x.num_maxalt).Sum();
            decimal avgMaxalt = Decimal.Round(totalMaxalt / new decimal(4), 2);
            decimal avgAleve = Decimal.Round(monthlyData.Select(x => x.num_aleve).Sum() / new decimal(4), 2);
            return new MonthStats { TotalHA = totalHA, TotalMaxalt = totalMaxalt, AvgMaxalt = avgMaxalt, AvgAleve = avgAleve };
        }


    }
}
