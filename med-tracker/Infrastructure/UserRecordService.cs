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
        private readonly IDataRepository dataRepository;

        public UserRecordService(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        public CommandResult StoreRecord(string commandText, string userID, string teamID)
        {
            var entries = commandText.Split(" ");
            if (ValidRecord(entries))
            {
                DateTime now = DateTime.Now;
                DateTime today = new DateTime(now.Year, now.Month, now.Day);
                long currentTime = new DateTimeOffset(today).ToUnixTimeSeconds();
                dataRepository.SetData(new DataDTO
                {
                    userID = userID,
                    teamID = teamID,
                    date = currentTime,
                    ha_present = entries[1] == "yes" ? true : false,
                    num_maxalt = Int32.Parse(entries[2]),
                    num_aleve = Int32.Parse(entries[3])
                });
                return new CommandResult { Error = false, ResultMessage = today.ToString("MM/dd/yy")};
            }
            else return new CommandResult { Error = true, ResultMessage = commandText };
        }

        public CommandResult UpdateRecord(string commandText, string userID, string teamID)
        {
            var entries = commandText.Split(" ");
            if (ValidRecord(entries))
            {
                dataRepository.SetData(new DataDTO
                {
                    userID = userID,
                    teamID = teamID,
                    date = new DateTimeOffset(DateTime.Parse(entries[1])).ToUnixTimeSeconds(),
                    ha_present = entries[2] == "yes" ? true : false,
                    num_maxalt = Int32.Parse(entries[3]),
                    num_aleve = Int32.Parse(entries[4])
                });
                return new CommandResult { Error = false, ResultMessage = entries[1] };
            }
            return new CommandResult { Error = true, ResultMessage = commandText };
        }

        private bool ValidRecord(string [] entries)
        {
            if (entries.Length != 4 && entries.Length != 5) return false;

            string maxalt = "";
            string aleve = "";
            string ha_present = "";

            if (entries.Length == 4)
            {
                ha_present = entries[1];
                maxalt = entries[2];
                aleve = entries[3];
            }
            if (entries.Length == 5)
            {
                ha_present = entries[2];
                maxalt = entries[3];
                aleve = entries[4];
                if (!DateTime.TryParse(entries[1], out DateTime date)) return false;
            }

            if (Int32.TryParse(maxalt, out int num_maxalt) && Int32.TryParse(aleve, out int num_aleve))
            {
                return (ha_present == "yes" || ha_present == "no") && num_maxalt >= 0 && num_aleve >= 0;
            }

            return false;
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
            if (results.Count() == 0) return new CommandResult { Error = true, ResultMessage = "No results available." };
            var resultObj = DataService.CalculateMonthlyStats(results);
            var resultMessage = $"Stats for the month.  Total # of headaches: {resultObj.TotalHA}, Total # Maxalt taken: {resultObj.TotalMaxalt} (Avg {resultObj.AvgMaxalt} per week), Avg Aleve per week: {resultObj.AvgAleve}";
            return new CommandResult { Error = false, ResultMessage = resultMessage };
        }

        private IEnumerable<DataDTO> RetrieveCurrentMonth(string userID, string teamID)
        {
            DateTime now = DateTime.Now;
            DateTime firstOfMonth = now.Day == 1 ? new DateTime(now.Year, (now.Month == 1 ? 12 : now.Month - 1), 1) : new DateTime(now.Year, now.Month, 1);
            long dayInSeconds = new DateTimeOffset(firstOfMonth).ToUnixTimeSeconds();
            return dataRepository.RetrieveMonthlyRecords(userID, teamID, dayInSeconds);
        }
    }
}
