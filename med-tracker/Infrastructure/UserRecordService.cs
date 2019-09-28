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
            var results = RetrieveCurrentMonth(userID, teamID).Select(
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
            var results = RetrieveCurrentMonth(userID, teamID);
            var month = Utilities.FormattedReportMonth(DateTime.Now);
            if (results.Count() == 0) return new CommandResult { Error = true, ResultMessage = $"Monthly report for {month}: No results available." };
            var resultObj = DataService.CalculateMonthlyStats(results);
            var resultMessage = $"Stats for {month}.  Total # of headaches: {resultObj.TotalHA}, Total # Maxalt taken: {resultObj.TotalMaxalt} (Avg {resultObj.AvgMaxalt} per week), Avg Aleve per week: {resultObj.AvgAleve}";
            return new CommandResult { Error = false, ResultMessage = resultMessage };
        }

        private IEnumerable<DataDTO> RetrieveCurrentMonth(string userID, string teamID)
        {
            long dayInSeconds = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            return userDataRepository.RetrieveMonthlyRecords(userID, teamID, dayInSeconds);
        }
    }
}
