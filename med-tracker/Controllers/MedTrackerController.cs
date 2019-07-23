using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace medtracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedTrackerController : ControllerBase
    {
        private readonly SlackAPI client;
        public MedTrackerController(SlackAPI client)
        {
            this.client = client;
        }

        // GET api/medtracker
        [HttpGet]
        public async Task TestMessage()
        {
            await client.SendMessage("<token>", "<channel>", "Here's a message");
        }

        [HttpGet("authorize")]
        public async Task GetAuthTokens()
        {
            string response = await client.Authorize("<client_id>", new List<string> {"bot", "command", );
        }
    }
}
