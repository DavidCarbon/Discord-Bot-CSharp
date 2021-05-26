using DiscordBot.BotCore.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.VoiceNext;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.BotCore
{
    public class Bot
    {
        public static DiscordClient Client { get; private set; }

        public static CommandsNextExtension Commands { get; private set; }

        public VoiceNextExtension Voice { get; private set; }

        public async Task RunBotAsync()
        {
            string json = string.Empty;
            using (var fileStream = File.OpenRead("config.json"))
            using (var streamReader = new StreamReader(fileStream, new UTF8Encoding(false)))
                json = await streamReader.ReadToEndAsync();

            /* So JSON data gets set when being read */
            var ConfigJson = JsonConvert.DeserializeObject<ConfigurationJson>(json);

            Client = new DiscordClient(BotConfiguration.Client);
            Voice = Client.UseVoiceNext();
            Client.UseInteractivity(BotConfiguration.Interactivity);

            Client.Ready += DiscordEvents.ClientReady;
            Client.GuildAvailable += DiscordEvents.GuildAvailable;
            /* Logging for Errors */
            Client.ClientErrored += DiscordEvents.ClientError;
            /* So we can Get the Bot to Respond to Commands */
            Commands = Client.UseCommandsNext(BotConfiguration.Commands);
            Commands.RegisterCommands<AdminCommands>();
            Commands.RegisterCommands<VoiceChatCommands>();

            await Client.ConnectAsync();

            //Client.MessageCreated += DiscordEvents.MessageHandler;

            await Task.Delay(-1);
        }
    }
}
