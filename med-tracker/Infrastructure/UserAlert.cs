using medtracker.Models;
using medtracker.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medtracker.Infrastructure
{
    public class UserAlert
    {
        private readonly UserTimesRepository userTimes;

        public UserAlert(UserTimesRepository userTimes)
        {
            this.userTimes = userTimes;
        }

        public CommandResult SetUserAlert(string commandText, string userID, string teamID)
        {
            var pingCommand = commandText.Split("at").ToList();
            string unParsedTime = pingCommand.Count() == 2 ? pingCommand[1] : "";
            if (DateTime.TryParse(unParsedTime, out DateTime parsedTime))
            {
                userTimes.SetUserTime(userID, teamID, Utilities.CalculateMinutes(parsedTime));
                return new CommandResult { Error = false, ResultMessage = parsedTime.ToString("hh:mm tt") };
            }
            return new CommandResult { Error = true, ResultMessage = unParsedTime };
        }
    }
}
