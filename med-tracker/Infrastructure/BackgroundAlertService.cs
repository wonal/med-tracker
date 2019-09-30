using medtracker.Config;
using medtracker.DTOs;
using medtracker.Models;
using medtracker.SQL;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace medtracker.Infrastructure
{

    public class BackgroundAlertService : BackgroundService
    {
        private int currentTime;
        private long firstOfMonth;
        private readonly SlackAPI slackAPI;
        private readonly IUserTimesRepository userPreferences;
        private readonly CredentialsRepository credentials;
        private readonly ISubscriberRepository subscriberRepository;
        private readonly MonthlyDataRepository monthlyDataRepository;
        private readonly UserRecordService userRecordService;

        public BackgroundAlertService(
            SlackAPI slackAPI, 
            IUserTimesRepository userPreferences, 
            CredentialsRepository credentials,
            ISubscriberRepository subscriberRepository,
            MonthlyDataRepository monthlyDataRepository,
            UserRecordService userRecordService)
        {
            this.slackAPI = slackAPI;
            this.userPreferences = userPreferences;
            this.subscriberRepository = subscriberRepository;
            this.monthlyDataRepository = monthlyDataRepository;
            this.userRecordService = userRecordService;
            this.credentials = credentials;
            currentTime = Utilities.CalculateSeconds(DateTime.Now);

            var time = DateTime.Now;
            firstOfMonth = time.Day == Constants.reportDay ? new DateTimeOffset(Utilities.NextReportDate(time)).ToUnixTimeSeconds() : new DateTimeOffset(Utilities.NextReportDate(time.AddMonths(1))).ToUnixTimeSeconds();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SendAlerts();
                    await SendReports();
                }
                catch (Exception) { /*todo: add logging*/}
                finally 
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
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

        public async Task SendReports()
        {
            var currentDay = DateTime.Now;
            var currentDayInSec = new DateTimeOffset(currentDay).ToUnixTimeSeconds();
            if (currentDayInSec >= firstOfMonth)
            {
                var users = userPreferences.GetUniqueUsers();
                foreach (UserTeam ut in users)
                {
                    var monthData = userRecordService.RetrieveRawStats(ut.userID, ut.teamID);
                    if(!monthData.Error)
                    {
                        monthlyDataRepository.StoreData(ut.userID, ut.teamID, monthData.Stats);
                    }
                }
                var subscribers = subscriberRepository.GetSubscribers();
                if (subscribers.Any())
                {
                    foreach (UserTeam ut in subscribers)
                    {
                        var report = userRecordService.RetrieveMonthStats(ut.userID, ut.teamID);
                        AuthResponseDTO teamInfo = credentials.GetValue(ut.teamID);
                        await slackAPI.SendMessage(teamInfo.bot.bot_access_token, ut.userID, report.ResultMessage);
                    }
                    var nextMonth = currentDay.AddMonths(1);
                    firstOfMonth = new DateTimeOffset(Utilities.NextReportDate(nextMonth)).ToUnixTimeSeconds();
                }
            }
        }
    }
}
