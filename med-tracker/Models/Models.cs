
namespace medtracker.Models
{
    public class CommandResult
    {
        public bool Error { get; set; }
        public string ResultMessage { get; set; }
    }
    public class UserTeam
    {
        public string userID { get; set; }
        public string teamID { get; set; }
    }
}
