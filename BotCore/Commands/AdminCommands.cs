using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.BotCore.Commands
{
    public class AdminCommands : BaseCommandModule
    {
        [Command("greet")]
        public async Task GreetCommand(CommandContext ctx)
        {
            await ctx.RespondAsync("Greetings! Thank you for executing me!");
        }

        [Command("Collect")]
        [Aliases("c")]
        public async Task CollectionCommand(CommandContext ctx)
        {
            var message = await ctx.RespondAsync("React here!");
            var reactions = await message.CollectReactionsAsync(TimeSpan.FromSeconds(10));

            var strBuilder = new StringBuilder();
            foreach (var reaction in reactions)
            {
                strBuilder.AppendLine($"{reaction.Emoji}: {reaction.Total}");
            }

            await ctx.RespondAsync(strBuilder.ToString());
        }

        [Command("OLDMuteVote")]
        [Aliases("oldmv")]
        [Description("Vote to Mute/Unmute a User in the connected Voice Channel")]
        public async Task MuteVoteCommand(CommandContext ctx, [Description("ID of User (Can be a @USER, DEV ID, Or Nick Name)")] DiscordMember Member)
        {
            try
            {
                bool MuteBoolean = (Member.IsMuted == true) ? true : false;
                string MuteVoteStatus = (MuteBoolean == true) ? "Vote to Unmute " + Member.Username : "Vote to Mute " + Member.Username;

                var Interactivity = ctx.Client.GetInteractivity();
                var Emoji = DiscordEmoji.FromName(ctx.Client, ":white_check_mark:");
                var PollEmbed = new DiscordEmbedBuilder
                {
                    Title = MuteVoteStatus,
                    Description = $"{MuteVoteStatus}, React with {Emoji} in 2 Minutes."
                };

                var PollMessage = await ctx.Channel.SendMessageAsync(embed: PollEmbed);

                await PollMessage.CreateReactionAsync(Emoji);

                try
                {
                    int ReactionResults = 0;
                    try
                    {
                        var Reactions = await Interactivity.CollectReactionsAsync(PollMessage, TimeSpan.FromSeconds(10));
                        ReactionResults = Reactions.Count() - 1;
                    }
                    catch (Exception Error)
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine(Error.Message + "\n");
                        Console.WriteLine(Error.InnerException + "\n");
                        Console.WriteLine(Error.StackTrace + "\n");
                        Console.WriteLine(Error.TargetSite + "\n");
                        Console.WriteLine("\n");
                        ReactionResults = 0;
                    }


                    string WhichStatus = (MuteBoolean == true) ? Member.Username + " Has been Voted to be no Longer Muted" : Member.Username + " Has been Voted to be Muted";

                    if (ReactionResults != 0)
                    {
                        WhichStatus = (MuteBoolean == true) ? Member.Username + " Has been Voted to be no Longer Muted" : Member.Username + " Has been Voted to be Muted";

                        string MuteStatus = (MuteBoolean == true) ? " has been Muted" : " has been Un-Muted";

                        try
                        {
                            await Member.SetMuteAsync(MuteBoolean);
                        }
                        catch
                        {
                            MuteStatus = "Unfortunely " + Member.Username + " Is not Connected to a Voice Call";
                        }
                    }
                    else
                    {
                        WhichStatus = (MuteBoolean == true) ? Member.Username + " Has been Voted to Remain Muted" : Member.Username + " Has been Voted to Remain Unmuted";
                    }

                    await ctx.Channel.SendMessageAsync(WhichStatus);
                }
                catch
                {
                    await ctx.RespondAsync("Reactions Error - Sorry Chief!");
                }
            }
            catch
            {
                await ctx.RespondAsync("Internal Bot Error - Sorry Chief!");
            }
        }
    }
}
