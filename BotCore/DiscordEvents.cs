using DSharpPlus;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DiscordBot.BotCore
{
    class DiscordPrefix
    {
        public static string Command = string.Empty;
    }

    class DiscordEvents
    {
        public static Task ClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            sender.Logger.LogInformation("Client is ready to process events.");

            return Task.CompletedTask;
        }

        public static Task GuildAvailable(DiscordClient sender, GuildCreateEventArgs e)
        {
            sender.Logger.LogInformation($"Guild available: {e.Guild.Name}");

            return Task.CompletedTask;
        }

        public static Task ClientError(DiscordClient sender, ClientErrorEventArgs e)
        {
            sender.Logger.LogError(e.Exception, "Exception occured");

            return Task.CompletedTask;
        }

        public static Task MessageHandler(DiscordClient sender, MessageCreateEventArgs args)
        {
            if (args.Message.Content.ToLower().StartsWith(ConfigurationJson.Prefix + "ping"))
                args.Message.RespondAsync("pong!");

            return Task.CompletedTask;
        }
    }
}
