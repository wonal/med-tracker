using medtracker.Config;
using medtracker.DTOs;
using medtracker.Infrastructure;
using medtracker.Models;

namespace medtracker
{
    public class CommandHandler
    {
        private readonly UserAlertService userAlertService;
        private readonly UserRecordService userRecordService;
        public CommandHandler(UserAlertService userAlertService, UserRecordService userRecordService)
        {
            this.userAlertService = userAlertService;
            this.userRecordService = userRecordService;
        }

        public string ParseCommand(SlashCommandDTO command)
        {
            string cmd = command.text.ToLower();
            if (cmd.Contains("ping me at"))
            {
                CommandResult alertResult = userAlertService.SetUserAlert(cmd, command.user_id, command.team_id);
                if (alertResult.Error) return $"{alertResult.ResultMessage} is an invalid format for time";
                else return $"You're all set!  I'll ping you each day at {alertResult.ResultMessage}";
            }
            else if (cmd.Contains("record"))
            {
                CommandResult recordResult = userRecordService.StoreRecord(cmd, command.user_id, command.team_id);
                if (recordResult.Error) return $"Sorry, '{recordResult.ResultMessage}' is in an invalid format";
                else return $"Data saved for {recordResult.ResultMessage}";
            }
            else if (cmd.Contains("update"))
            {
                CommandResult recordResult = userRecordService.UpdateRecord(cmd, command.user_id, command.team_id);
                if (recordResult.Error) return $"Sorry, '{recordResult.ResultMessage}' is in an invalid format";
                else return $"Data updated for {recordResult.ResultMessage}";
            }
            else if (cmd == "stats")
            {
                return userRecordService.RetrieveMonthStats(command.user_id, command.team_id).ResultMessage;
            }
            else if (cmd == ("raw"))
            {
                return userRecordService.RetrieveMonthsRecords(command.user_id, command.team_id).ResultMessage;
            }
            else if (cmd.Contains("stop"))
            {
                CommandResult stopResult = userAlertService.DeleteUserAlert(cmd, command.user_id, command.team_id);
                if (stopResult.Error) return $"{stopResult.ResultMessage}";
                else return "Noted! I'll stop pinging you starting today.";
            }
            return $"Sorry, I do not understand the command '{command.text}'";
        }
    }
}
