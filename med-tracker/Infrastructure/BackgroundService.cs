using medtracker.DTOs;
using medtracker.Models;
using medtracker.SQL;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace medtracker
{
    public class BackgroundService : IHostedService, IDisposable
    {
        private Timer timer;
        private int currentTime;
        private readonly SlackAPI slackAPI;
        private readonly IUserTimesRepository userPreferences;
        private readonly CredentialsRepository credentials;

        public BackgroundService(SlackAPI slackAPI, IUserTimesRepository userPreferences, CredentialsRepository credentials)
        {
            this.slackAPI = slackAPI;
            this.userPreferences = userPreferences;
            this.credentials = credentials;
            currentTime = Utilities.CalculateSeconds(DateTime.Now);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(SendAlerts, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));  
            return Task.CompletedTask;
        }

        private async void SendAlerts(Object state)
        {
            var nextAlarm = Utilities.CalculateSeconds(DateTime.Now);
            var users = userPreferences.GetUsers(currentTime, nextAlarm);
            foreach (UserTeam ut in users)
            {
                AuthResponseDTO teamInfo = credentials.GetValue(ut.teamID);
                await slackAPI.SendMessage(teamInfo.bot.bot_access_token, ut.userID, $"Your alert!");
            }
            currentTime = nextAlarm;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (timer != null)
            {
                timer.Change(Timeout.Infinite, 0);
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if(timer != null) timer.Dispose();
        }
    }
}
