using medtracker.DTOs;
using medtracker.SQL;
using System;
using System.Linq;

namespace medtracker
{
    public class CommandHandler
    {
        private readonly UserTimesRepository userPreferences;
        private readonly BackgroundService service;
        public CommandHandler(UserTimesRepository userPreferences, BackgroundService service)
        {
            this.userPreferences = userPreferences;
            this.service = service;
        }

        public string ParseCommand(SlashCommandDTO command)
        {
            if (command.text.ToLower().Contains("ping me @"))
            {
                string unFormattedTime = command.text.Split("@").ToList()[1];
                if (DateTime.TryParse(unFormattedTime, out DateTime parsedTime))
                {
                    string time = parsedTime.ToString("HH:mm");
                    string userTime = parsedTime.ToString("hh:mm tt");
                    userPreferences.SetUserTime(command.user_id, command.team_id, time);
                    return $"You're all set!  I'll ping you each day at {userTime}";
                }
                else return $"{unFormattedTime} is an invalid format for time";
            }
            return $"Sorry, I do not understand the command {command.text}";
        }
    }
}
