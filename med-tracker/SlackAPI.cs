using medtracker.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace medtracker
{
    public class SlackAPI : ISlackAPI
    {
        public static HttpClient GetHTTPClient(string token)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://slack.com/api/");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            return client;
        }

        public async Task SendMessage(string token, string channelID, string message)
        {
            var client = GetHTTPClient(token);
            var data = new MessageDTO { channel = channelID, text = message };
            var postContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("chat.postMessage", postContent);
        }
    }
}
