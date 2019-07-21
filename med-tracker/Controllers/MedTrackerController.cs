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

        // GET api/medtracker
        [HttpGet]
        public async Task Get()
        {
            SlackAPI client = new SlackAPI();
            await client.SendMessage("token", "channel", "A message");
        }
    }
}
