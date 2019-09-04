using medtracker.DTOs;
using medtracker.Infrastructure;
using medtracker.Models;

namespace medtracker
{
    public class CommandHandler
    {
        private readonly UserAlert userAlert;
        private readonly UserRecord userRecord;
        public CommandHandler(UserAlert userAlert, UserRecord userRecord)
        {
            this.userAlert = userAlert;
            this.userRecord = userRecord;
        }

        public string ParseCommand(SlashCommandDTO command)
        {
            string cmd = command.text.ToLower();
            if (cmd.Contains("ping me at"))
            {
                CommandResult alertResult = userAlert.SetUserAlert(cmd, command.user_id, command.team_id);
                if (alertResult.Error) return $"{alertResult.ResultMessage} is an invalid format for time";
                else return $"You're all set!  I'll ping you each day at {alertResult.ResultMessage}";
            }
            else if (cmd.Contains("record"))
            {
                CommandResult recordResult = userRecord.StoreRecord(cmd, command.user_id, command.team_id);
                if (recordResult.Error) return $"Sorry, '{recordResult.ResultMessage}' is in an invalid format";
                else return $"Data saved for {recordResult.ResultMessage}";
            }
            return $"Sorry, I do not understand the command '{command.text}'";
        }
    }
}
