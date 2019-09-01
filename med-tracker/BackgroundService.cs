using medtracker.DTOs;
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
        private int nextAlarm;
        private readonly SlackAPI slackAPI;
        private readonly UserTimesRepository userPreferences;
        private readonly CredentialsRepository credentials;

        public BackgroundService(SlackAPI slackAPI, UserTimesRepository userPreferences, CredentialsRepository credentials)
        {
            this.slackAPI = slackAPI;
            this.userPreferences = userPreferences;
            this.credentials = credentials;
            currentTime = Utilities.CalculateMinutes(DateTime.Now);
            nextAlarm = currentTime + 1;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(SendAlerts, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private async void SendAlerts(Object state)
        {
            var users = userPreferences.GetUsers(currentTime, nextAlarm);
            if (users.Count() > 0)
            {
                foreach (UserTeamDTO ut in users)
                {
                    AuthResponseDTO teamInfo = credentials.GetValue(ut.teamID);
                    await slackAPI.SendMessage(teamInfo.bot.bot_access_token, ut.userID, $"Your alert!");
                }
            }
            currentTime = nextAlarm;
            nextAlarm += 1;
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
