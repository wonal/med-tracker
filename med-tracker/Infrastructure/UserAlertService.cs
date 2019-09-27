using medtracker.Config;
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
        private readonly ISubscriberRepository subscriberRepository;

        public UserAlertService(IUserTimesRepository userTimes, ISubscriberRepository subscriberRepository)
        {
            this.userTimes = userTimes;
            this.subscriberRepository = subscriberRepository;
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

        public CommandResult SetUpSubscription(string userId, string teamId)
        {
            subscriberRepository.SetSubscriber(userId, teamId);
            var time = DateTime.Now;
            var reportDay = time.Day == Constants.reportDay ? time.ToString("d") : time.AddMonths(1).ToString("d");
            return new CommandResult { Error = false, ResultMessage = $"Subscribed! Next report: {reportDay}" };
        }

        public CommandResult DeleteSubscription(string userId, string teamId)
        {
            if (subscriberRepository.GetSubscriber(userId, teamId).Any())
            {
                subscriberRepository.DeleteSubscriber(userId, teamId);
                return new CommandResult { Error = false, ResultMessage = "Successfully unsubscribed" };
            }
            else return new CommandResult { Error = true, ResultMessage = "No subscription set up yet" };

        }
    }
}
