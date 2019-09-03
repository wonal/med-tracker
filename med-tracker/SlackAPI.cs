using medtracker.Config;
using medtracker.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace medtracker
{
    public class SlackAPI 
    {
        private readonly HttpClient client;
        public SlackAPI()
        {
            client = new HttpClient();
        }

        public async Task<AuthResponseDTO> Authorize(string code)
        {
            var uri = Constants.slack_uri + "/api/oauth.access";
            var content = new Dictionary<string, string>
            {
                { "client_id", ClientKeys.client_id },
                { "client_secret", ClientKeys.client_secret },
                { "code", code },
                { "redirect_uri", Constants.redirect_uri }
            };

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            requestMessage.Content = new FormUrlEncodedContent(content);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await client.SendAsync(requestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AuthResponseDTO>(responseContent);
            return result;
        }

        public async Task SendMessage(string token, string channelID, string message)
        {

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://slack.com/api/chat.postMessage");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var data = new MessageDTO { channel = channelID, text = message };
            requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.SendAsync(requestMessage);
        }
    }
}
