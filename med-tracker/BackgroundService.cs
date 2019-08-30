using medtracker.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace medtracker
{
    public class BackgroundService
    {
        /*
        will run a timer that goes off once a minute and checks to see if a user needs to be pinged.  
        first iteration: goes off once every ten seconds and messages hard-coded user
         */
        public Timer Timer { get; private set; }
        private readonly SlackAPI slackAPI;
        private readonly UserTimesRepository userPreferences;
        public BackgroundService(SlackAPI slackAPI, UserTimesRepository userPreferences)
        {
            this.slackAPI = slackAPI;
            this.userPreferences = userPreferences;

            Timer = new Timer(20000);
            Timer.Elapsed += SendAlerts;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        private async void SendAlerts(Object source, ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("HH:mm");
            var users = userPreferences.GetUsers(time);
            if (users.Count() > 0)
            {
                var user = users.FirstOrDefault();
                await slackAPI.SendMessage("<bot token>", user, $"It's {now.ToString("hh:mm")}pm!");
            }
        }
    }
}
