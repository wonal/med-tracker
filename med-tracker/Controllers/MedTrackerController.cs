using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using medtracker.Config;
using medtracker.DTOs;
using medtracker.SQL;
using Microsoft.AspNetCore.Mvc;

namespace medtracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedTrackerController : ControllerBase
    {
        private readonly SlackAPI client;
        private readonly CredentialsRepository repository;
        public MedTrackerController(SlackAPI client, CredentialsRepository repository)
        {
            this.client = client;
            this.repository = repository;
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
            repository.SetCredentials(response.team_id, response);
            return Ok();
        }

        [HttpPost("mt")]
        public IActionResult TrackMeds(TrackCommandDTO commandDTO)
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
            return Ok("todo");
        }
    }
}
