using medtracker.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medtracker
{
    interface ISlackAPI
    {
        Task SendMessage(string token, string channelID, string message);
    }
}
