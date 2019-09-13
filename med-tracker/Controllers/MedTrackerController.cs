using medtracker.Config;
using medtracker.DTOs;
using medtracker.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace medtracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedTrackerController : ControllerBase
    {
        private readonly CredentialsRepository repository;
        private readonly CommandHandler handler;
        private readonly SlackAPI slackApi;
        public MedTrackerController(CommandHandler handler, CredentialsRepository repository, SlackAPI slackApi)
        {
            this.handler = handler;
            this.repository = repository;
            this.slackApi = slackApi;
        }

        // GET api/medtracker
        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok();
        }

        [HttpPost("event")]
        public async Task<IActionResult> EventTriggered(EventDTO eventData)
        {

            if (eventData.eventType.type == "message" && eventData.eventType.text.Contains("hello world"))
            {
                var token = repository.GetValue(eventData.team_id).bot.bot_access_token;
                await slackApi.SendMessage(token, eventData.eventType.channel, "Your message was received!");
            }
            return Ok();
        }

        [HttpPost("slackaccess")]
        public IActionResult GetAuthTokens(AuthResponseDTO response)
        {
            repository.SetValue(response.team_id, response);
            return Ok();
        }

        /*
        [HttpGet("slackaccess")]
        public async Task<IActionResult> GetAuthTokens(string code)
        {
            AuthResponseDTO response = await client.Authorize(code);
            repository.SetValue(response.team_id, response);
            return Ok();
        }*/

        [HttpPost]
        public IActionResult TrackMeds([FromForm] SlashCommandDTO slashCommand)
        {
            /*
                        int timestamp = int.Parse(Request.Headers["X-Slack-Request-Timestamp"]);
                        if (DateTimeOffset.Now - DateTimeOffset.FromUnixTimeSeconds(timestamp) > TimeSpan.FromMinutes(5))
                        {
                            return BadRequest();
                        }
                        string verificationString = Request.Headers["X-Slack-Signature"];
                        string signature = $"v0:{timestamp}:{Request.Body}";
            */
            string response = handler.ParseCommand(slashCommand);
            return Ok(response);
        }

        [HttpPost("rawdata")]
        public IActionResult RawMonthRecords([FromForm] RecordRequestDTO request)
        {
            string token = Request.Headers["Authorization"];
            if (token != $"Bearer {ClientKeys.password}") return BadRequest();
            return Ok(handler.ParseCommand(new SlashCommandDTO { text = "month", user_id = request.UserID, team_id = request.TeamID }));
        }
    }
}
