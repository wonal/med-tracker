using medtracker.DTOs;
using medtracker.Models;
using medtracker.SQL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace medtracker.Infrastructure
{
    public class UserRecordService
    {
        private readonly IUserDataRepository userDataRepository;

        public UserRecordService(IUserDataRepository userDataRepository)
        {
            this.userDataRepository = userDataRepository;
        }

        public CommandResult StoreRecord(string commandText, string userID, string teamID)
        {
            var entries = commandText.Split(" ");
            var record = UserRecord.CreateRecord(entries);
            if (record.Error)
                return new CommandResult { Error = true, ResultMessage = commandText };

            DateTime now = DateTime.Now;
            DateTime today = new DateTime(now.Year, now.Month, now.Day);
            long currentTime = new DateTimeOffset(today).ToUnixTimeSeconds();
            userDataRepository.SetData(new DataDTO
            {
                userID = userID,
                teamID = teamID,
                date = currentTime,
                ha_present = record.HaPresent.Value,
                num_maxalt = record.NumMaxalt.Value,
                num_aleve = record.NumAleve.Value
            });
            return new CommandResult { Error = false, ResultMessage = today.ToString("MM/dd/yy") };
        }

        public CommandResult UpdateRecord(string commandText, string userID, string teamID)
        {
            var entries = commandText.Split(" ");
            var record = UserRecord.CreateRecord(entries);
            if (record.Error)
                return new CommandResult { Error = true, ResultMessage = commandText };

            userDataRepository.SetData(new DataDTO
            {
                userID = userID,
                teamID = teamID,
                date = new DateTimeOffset(record.Date.Value).ToUnixTimeSeconds(),
                ha_present = record.HaPresent.Value,
                num_maxalt = record.NumMaxalt.Value,
                num_aleve = record.NumAleve.Value
            });
            return new CommandResult { Error = false, ResultMessage = entries[1] };
        }
        public CommandResult RetrieveMonthsRecords(string userID, string teamID)
        {
            long dayInSeconds = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            var results =  userDataRepository.GetLastThirtyRecords(userID, teamID, dayInSeconds).Select(
                r => new UserRecordDTO {
                    Date = DateTimeOffset.FromUnixTimeSeconds(r.date).ToString("d"),
                    HA_Present = r.ha_present,
                    Num_Maxalt = r.num_maxalt,
                    Num_Aleve = r.num_aleve });

            if (results.Count() == 0) return new CommandResult { Error = true, ResultMessage = "No results available." };
            return new CommandResult { Error = false, ResultMessage = JsonConvert.SerializeObject(results) };
        }

        public CommandResult RetrieveMonthStats(string userID, string teamID)
        {
            var resultObj = RetrieveRawStats(userID, teamID);
            if (resultObj.Error) return new CommandResult { Error = true, ResultMessage = $"No results available for this month's report." };
            var month = DateTime.Parse(resultObj.Stats.Month);
            var resultMessage = $"Stats for {month.ToString("MMMM")}.  Total # of headaches: {resultObj.Stats.TotalHa}, Avg of {resultObj.Stats.AvgMaxalt} Maxalt per week, Avg Aleve per week: {resultObj.Stats.AvgAleve}";
            return new CommandResult { Error = false, ResultMessage = resultMessage };
        }

        public DataResult RetrieveRawStats(string userID, string teamID)
        {
            var month = GetFirstOfMonth(DateTime.Now);
            long startDayInSeconds = new DateTimeOffset(month).ToUnixTimeSeconds();
            var results = userDataRepository.GetCurrentMonthRecords(userID, teamID, startDayInSeconds);
            if (!results.Any()) return new DataResult { Error = true, Stats = null };
            var resultObj = DataService.CalculateMonthlyStats(results);
            return new DataResult { Error = false, Stats = new MonthlyData { Month = month.ToString("yyyy/MM"), TotalHa = resultObj.TotalHA, AvgMaxalt = resultObj.AvgMaxalt, AvgAleve = resultObj.AvgAleve } };
        }

        //Returns the first of the month unless the current day is the first of the month, in which case it returns previous month
        private static DateTime GetFirstOfMonth(DateTime time)
        {
            if (time.Day == 1)
            {
                time.AddMonths(-1);
                return new DateTime(time.Year, time.Month, time.Day);
            }
            return new DateTime(time.Year, time.Month, 1);
        }
    }
}
