
namespace medtracker.Models
{
    public class CommandResult
    {
        public bool Error { get; set; }
        public string ResultMessage { get; set; }
    }
    public class UserTeam
    {
        public string userID { get; set; }
        public string teamID { get; set; }
    }

    public class MonthStats
    {
        public int TotalHA { get; set; }
        public int TotalMaxalt { get; set; }
        public decimal AvgMaxalt { get; set; }
        public decimal AvgAleve { get; set; }
    }

    public class MonthlyData
    {
        public string Month { get; set; }
        public int TotalHa { get; set; }
        public decimal AvgMaxalt { get; set; }
        public decimal AvgAleve { get; set; }
    }

    public class DataResult
    {
        public bool Error { get; set; }
        public MonthlyData Stats { get; set; }
    }
}
