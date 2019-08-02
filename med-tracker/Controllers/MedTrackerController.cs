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
        public async Task TestMessage()
        {
            await client.SendMessage("<token>", "<channel>", "Here's a message");
        }

        [HttpGet("slackaccess")]
        public async Task GetAuthTokens(string code)
        {
            AuthResponseDTO response = await client.Authorize(code);
            repository.SetCredentials(response.team_id, response);
        }

        /*
        [HttpPost("track")]
        public async Task TrackMeds(TrackCommandDTO commandDTO)
        {

        }
        */
    }
}
