﻿
namespace medtracker.Config
{
    public static class Constants
    {
        public const string slack_uri = "https://slack.com";
        public const string redirect_uri = "https://localhost:5001/api/medtracker/slackaccess";
        public const string scopes = "channels:read,chat:write:bot,incoming-webhook,bot,commands";
    }
}