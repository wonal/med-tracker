using medtracker.DTOs;
using medtracker.SQL;
using Microsoft.AspNetCore.Mvc;

namespace medtracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedTrackerController : ControllerBase
    {
        private readonly CredentialsRepository repository;
        private readonly CommandHandler handler;
        public MedTrackerController(CommandHandler handler, CredentialsRepository repository)
        {
            this.handler = handler;
            this.repository = repository;
        }

        // GET api/medtracker
        [HttpGet]
        public IActionResult HealthCheck()
        {
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
    }
}
