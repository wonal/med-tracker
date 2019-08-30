using medtracker.SQL;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace medtracker
{
    public class BackgroundService : IHostedService, IDisposable
    {
        /*
        will run a timer that goes off once a minute and checks to see if a user needs to be pinged.  
        first iteration: goes off once every ten seconds and messages hard-coded user
         */
        private Timer timer;
        private readonly SlackAPI slackAPI;
        private readonly UserTimesRepository userPreferences;
        public BackgroundService(SlackAPI slackAPI, UserTimesRepository userPreferences)
        {
            this.slackAPI = slackAPI;
            this.userPreferences = userPreferences;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(SendAlerts, null, TimeSpan.Zero, TimeSpan.FromSeconds(20));
            return Task.CompletedTask;
        }

        private async void SendAlerts(Object state)
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

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer.Dispose();
        }
    }
}
