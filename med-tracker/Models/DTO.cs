
using Newtonsoft.Json;

namespace medtracker.DTOs
{
    public class MessageDTO
    {
        public string channel { get; set; }
        public string text { get; set; }
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

    public class SlashCommandDTO
    {
        public string text { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string team_id { get; set; }
        public string response_url { get; set; }
    }


    public class DataDTO
    {
        public string userID { get; set; }
        public string teamID { get; set; }
        public long date { get; set; }
        public bool ha_present { get; set; }
        public int num_maxalt { get; set; }
        public int num_aleve { get; set; }
    }
    public class ContentDTO
    {
        public string type { get; set;}
        public string channel { get; set; }
        public string user { get; set; }
        public string text { get; set; }
        public string ts { get; set; }
    }

    public class EventDTO
    {
        public string team_id { get; set; }

        [JsonProperty("event")]
        public ContentDTO eventType { get; set; }
        public string type { get; set; }
    }

}
