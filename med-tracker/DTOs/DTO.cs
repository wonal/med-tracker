
namespace medtracker.DTOs
{
    public class MessageDTO
    {
        public string channel { get; set; }
        public string text { get; set; }
    }

    public class AccessDTO
    {
        public string access_token { get; set; }
        public string scope { get; set; }
    }

    public class BotDTO
    {
        public string bot_user_id { get; set; }
        public string bot_access_token { get; set; }
    }

    public class AuthResponseDTO
    {
        public string access_token { get; set; }
        public string scope { get; set; }
        public string team_name { get; set; }
        public string team_id { get; set; }
        public BotDTO bot { get; set; }
    }
}
