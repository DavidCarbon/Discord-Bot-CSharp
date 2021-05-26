using DiscordBot.BotCore;

namespace DiscordBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Bot prog = new Bot();
            prog.RunBotAsync().GetAwaiter().GetResult();
        }
    }
}
