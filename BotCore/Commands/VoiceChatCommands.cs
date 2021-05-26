using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;

namespace DiscordBot.BotCore.Commands
{
    class VoiceChatCommands : BaseCommandModule
    {
        [Command("mute")]
        [RequireUserPermissions(Permissions.MuteMembers)]
        [Description("Mutes a User in a Voice Channel")]
        public async Task MuteCommand(CommandContext ctx, DiscordMember member)
        {
            bool MuteBoolean = false;
            string MuteStatus = "Muted";

            if (member.IsMuted == true)
            {
                MuteBoolean = false;
                MuteStatus = "UnMuted";
            }
            await member.SetMuteAsync(MuteBoolean);
            await ctx.RespondAsync($"{member.Username} has been " + MuteStatus);
        }

        [Command("amimuted")]
        [Description("Mutes a User in a Voice Channel")]
        public async Task MuteStatusCommand(CommandContext ctx, DiscordMember member)
        {
            string MuteStatus = "Not Muted";
            if (member.IsMuted == true)
            {
                MuteStatus = "Muted";
            }
            await ctx.RespondAsync("You are " + MuteStatus);
        }
    }
}
