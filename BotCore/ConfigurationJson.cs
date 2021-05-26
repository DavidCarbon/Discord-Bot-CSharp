using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace DiscordBot.BotCore
{
    class BotConfiguration
    {
        public static InteractivityConfiguration Interactivity = new InteractivityConfiguration
        {
            PollBehaviour = PollBehaviour.KeepEmojis,
            Timeout = TimeSpan.FromSeconds(30)
        };

        public static DiscordConfiguration Client = new DiscordConfiguration
        {
            Token = ConfigurationJson.Token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All,

            AutoReconnect = true,
            MinimumLogLevel = LogLevel.Debug
        };

        public static CommandsNextConfiguration Commands = new CommandsNextConfiguration
        {
            StringPrefixes = new string[] { ConfigurationJson.Prefix },
            EnableDms = false,
            EnableMentionPrefix = true,
            DmHelp = false
        };
    }

    public struct ConfigurationJson
    {
        [JsonProperty("token")]
        public static string Token { get; private set; }

        [JsonProperty("prefix")]
        public static string Prefix { get; private set; }
    }
}
