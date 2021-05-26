using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.VoiceNext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.BotCore.Commands
{
    class VoiceChatCommands : BaseCommandModule
    {
        [Command("Mute"), Aliases("m")]
        [RequireUserPermissions(Permissions.MuteMembers)]
        [Description("Mutes a User in a Voice Channel")]
        public async Task MuteCommand(CommandContext ctx, [Description("ID of User (Can be a @USER, DEV ID, Or Nickname)")] DiscordMember Member)
        {
            try
            {
                bool MuteBoolean = (Member.IsMuted == true) ? true : false;
                string MuteStatus = (MuteBoolean == true) ? " has been Muted" : " has been Un-Muted";

                try
                {
                    await Member.SetMuteAsync(MuteBoolean);
                }
                catch
                {
                    MuteStatus = " is not in a Voice Channel";
                }
                
                await ctx.RespondAsync($"{Member.Username}" + MuteStatus);
            }
            catch
            {
                await ctx.RespondAsync("Internal Bot Error - Sorry Chief!");
            }
        }

        [Command("MuteVote")]
        [Aliases("mv")]
        [Description("Vote to Mute/Unmute a User in the connected Voice Channel")]
        public async Task MuteVoteCommand(CommandContext ctx, [Description("ID of User (Can be a @USER, DEV ID, Or Nick Name)")] DiscordMember Member)
        {
            try
            {
                TimeSpan Timer = TimeSpan.FromSeconds(10);
                bool MuteBoolean = (Member.IsMuted == true) ? true : false;
                string MuteVoteStatus = (MuteBoolean == true) ? "Vote to Unmute " + Member.Username : "Vote to Mute " + Member.Username;

                var Interactivity = ctx.Client.GetInteractivity();
                var Emoji = DiscordEmoji.FromName(ctx.Client, ":white_check_mark:");
                var Message = await ctx.Channel.SendMessageAsync($"{MuteVoteStatus}, React with {Emoji} in 2 Minutes.");

                await Message.CreateReactionAsync(Emoji);

                try
                {
                    int ReactionResults;
                    try
                    {
                        var Reactions = await Interactivity.CollectReactionsAsync(Message, Timer);
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

                        bool NewBoolen = (MuteBoolean == true) ? false : true;
                        string MuteStatus = (NewBoolen == true) ? " has been Muted" : " has been Un-Muted";

                        try
                        {
                            await Member.SetMuteAsync(NewBoolen);
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

        /* Check if a User is Muted */
        [Command("MuteCheck"), Aliases("mc")]
        [Description("Checks if a User is Muted or Not")]
        public async Task MuteStatusCommand(CommandContext ctx, DiscordMember Member)
        {
            string UserName = (string.IsNullOrEmpty(Member.Nickname)) ? Member.Username : Member.Nickname;
            string MuteStatus = (Member.IsMuted == true) ? "Muted" : "Not Muted";
            await ctx.RespondAsync(UserName + " is currently " + MuteStatus);
        }

        /* Check if the Self-User is Muted */
        [Command("MuteCheck"), Aliases("mcs")]
        [Description("Checks if a User is Muted or Not")]
        public async Task MuteStatusCommand(CommandContext ctx)
        {
            DiscordMember Member = (DiscordMember)ctx.User;
            string MuteStatus = (Member.IsMuted == true) ? "Muted" : "Not Muted";
            await ctx.RespondAsync(ctx.User.Username + " is currently " + MuteStatus);
        }

        /* Mutes an Entire Call from the User's Connected Voice Channel */
        [Command("MuteAll"), Aliases("ma")]
        [Description("Mutes all users who are on the same voice channel as you")]
        [RequirePermissions(Permissions.MuteMembers)]
        public async Task MuteAll(CommandContext ctx)
        {
            try
            {
                if (ctx.Guild == null)
                {
                    await ctx.Channel.SendMessageAsync($"{ctx.User.Mention} You are not on a server :O").ConfigureAwait(false);
                    return;
                }

                VoiceNextExtension vnext = ctx.Client.GetVoiceNext();
                VoiceNextConnection vnc = vnext.GetConnection(ctx.Guild);
                bool UserIsInAChannel = false;

                if (vnc == null)
                {
                    var chn = ctx.Member?.VoiceState?.Channel;
                    if (chn == null)
                    {
                        await ctx.Channel.SendMessageAsync($"{ctx.User.Mention} You need to be on a voice channel to use this command");
                        return;
                    }
                    else
                    {
                        UserIsInAChannel = true;
                    }
                }

                if (UserIsInAChannel == true)
                {
                    int UsersInCurrentCall = 0;

                    if (ctx.Member?.VoiceState?.Channel.Users != null)
                    {
                        foreach (DiscordMember Members in ctx.Member?.VoiceState?.Channel.Users)
                        {
                            if (!Members.IsBot)
                            {
                                UsersInCurrentCall++;
                                await Members.SetMuteAsync(true);
                            }
                        }
                    }

                    await ctx.Channel.SendMessageAsync($"{ctx.User.Mention} All (" + UsersInCurrentCall + ") users have been muted!").ConfigureAwait(false);
                }
                else
                {
                    await ctx.Channel.SendMessageAsync($"{ctx.User.Mention} Internal Error Occured while Trying to Run This Command");
                }
            }
            catch
            {
                await ctx.Channel.SendMessageAsync("Internal Bot Error - Sorry Chief!");
            }
        }
    }
}
