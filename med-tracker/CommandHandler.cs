using medtracker.DTOs;
using medtracker.SQL;
using System;
using System.Linq;

namespace medtracker
{
    public class CommandHandler
    {
        private readonly UserTimesRepository userPreferences;
        public CommandHandler(UserTimesRepository userPreferences)
        {
            this.userPreferences = userPreferences;
        }

        public string ParseCommand(SlashCommandDTO command)
        {
            if (command.text.ToLower().Contains("ping me @"))
            {
                string unFormattedTime = command.text.Split("@").ToList()[1];
                if (DateTime.TryParse(unFormattedTime, out DateTime parsedTime))
                {
                    string userTime = parsedTime.ToString("hh:mm tt");
                    userPreferences.SetUserTime(command.user_id, command.team_id, Utilities.CalculateMinutes(parsedTime));
                    return $"You're all set!  I'll ping you each day at {userTime}";
                }
                else return $"{unFormattedTime} is an invalid format for time";
            }
            return $"Sorry, I do not understand the command {command.text}";
        }
    }
}
