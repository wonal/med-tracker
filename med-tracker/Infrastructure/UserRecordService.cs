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

        private bool ValidRecord(string [] entries)
        {
            if (entries.Length != 4) return false;
            if(Int32.TryParse(entries[2], out int maxalt) && Int32.TryParse(entries[3], out int aleve))
            {
                return entries[0] == "record" && (entries[1] == "yes" || entries[1] == "no") &&
                    maxalt >= 0 && aleve >= 0;

            }
            return false;
        }

        public CommandResult RetrieveMonthsRecords(string userID, string teamID)
        {
            DateTime now = DateTime.Now;
            DateTime firstOfMonth = new DateTime(now.Year, now.Month, 1);
            long dayInSeconds = new DateTimeOffset(firstOfMonth).ToUnixTimeSeconds();
            var results = dataRepository.RetrieveMonthlyRecords(userID, teamID, dayInSeconds).Select(
                r => new UserRecordDTO {
                    Date = DateTimeOffset.FromUnixTimeSeconds(r.date).ToString("g"),
                    HA_Present = r.ha_present,
                    Num_Maxalt = r.num_maxalt,
                    Num_Aleve = r.num_aleve });

            if (results.Count() == 0) return new CommandResult { Error = true, ResultMessage = "No results available." };
            return new CommandResult { Error = false, ResultMessage = JsonConvert.SerializeObject(results) };
        }
    }
}
