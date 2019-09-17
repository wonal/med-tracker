using medtracker.DTOs;
using medtracker.Models;
using medtracker.SQL;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace medtracker.Infrastructure
{

    public class BackgroundAlertService : BackgroundService
    {
        private int currentTime;
        private readonly SlackAPI slackAPI;
        private readonly IUserTimesRepository userPreferences;
        private readonly CredentialsRepository credentials;

        public BackgroundAlertService(SlackAPI slackAPI, IUserTimesRepository userPreferences, CredentialsRepository credentials)
        {
            this.slackAPI = slackAPI;
            this.userPreferences = userPreferences;
            this.credentials = credentials;
            currentTime = Utilities.CalculateSeconds(DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await SendAlerts();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task SendAlerts()
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
    }
}
