using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DiscordBot.BotCore
{
    class BotConfiguration
    {
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
