using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using medtracker.Config;
using medtracker.DTOs;
using medtracker.SQL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace medtracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedTrackerController : ControllerBase
    {
        private readonly SlackAPI client;
        private readonly CredentialsRepository repository;
        private readonly UserTimesRepository timeRepository;
        public MedTrackerController(SlackAPI client, CredentialsRepository repository, UserTimesRepository timeRepository)
        {
            this.client = client;
            this.repository = repository;
            this.timeRepository = timeRepository;
        }

        // GET api/medtracker
        [HttpGet]
        public async Task<IActionResult> TestMessage()
        {
            await client.SendMessage("<token>", "<channel>", "Here's a message");
            return Ok();
        }

        [HttpGet("slackaccess")]
        public async Task<IActionResult> GetAuthTokens(string code)
        {
            AuthResponseDTO response = await client.Authorize(code);
            repository.SetValue(response.team_id, response);
            return Ok();
        }

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
            timeRepository.SetUserTime(slashCommand.user_id, new UserPreferenceDTO { team_id = slashCommand.team_id, time = 9 });
            var users = timeRepository.GetUsers(9);
            return Ok($"{users.Count()}");
        }
    }
}
