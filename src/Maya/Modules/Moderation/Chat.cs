using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Maya.Modules.Moderation
{
    public class Chat : ModuleBase<SocketCommandContext>
    {
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        [Command("clean")]
        [Summary("Delete the last specified number of messages sent in the current Channel.")]
        public async Task Clean(int messages)
        {
            await Context.Message.DeleteAsync();
            var msgs = await Context.Channel.GetMessagesAsync(messages).Flatten();
            await Context.Channel.DeleteMessagesAsync(msgs);
        }
    }
}