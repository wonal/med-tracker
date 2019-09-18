using medtracker.Models;
using medtracker.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medtracker.Infrastructure
{
    public class UserAlertService
    {
        private readonly IUserTimesRepository userTimes;

        public UserAlertService(IUserTimesRepository userTimes)
        {
            this.userTimes = userTimes;
        }

        public CommandResult SetUserAlert(string commandText, string userID, string teamID)
        {
            var pingCommand = commandText.Split("at").ToList();
            string unParsedTime = pingCommand.Count() == 2 ? pingCommand[1] : "";
            if (DateTime.TryParse(unParsedTime, out DateTime parsedTime))
            {
                userTimes.SetUserTime(userID, teamID, Utilities.CalculateSeconds(parsedTime));
                return new CommandResult { Error = false, ResultMessage = parsedTime.ToString("hh:mm tt") };
            }
            return new CommandResult { Error = true, ResultMessage = unParsedTime };
        }

        public CommandResult DeleteUserAlert(string commandText, string userID, string teamID)
        {
            if (userTimes.GetUserTime(userID, teamID).Any())
            {
                userTimes.DeleteUserTime(userID, teamID);
                return new CommandResult { Error = false, ResultMessage = commandText };
            }
            else return new CommandResult { Error = true, ResultMessage = "No alert set up yet" };
        }
    }
}
