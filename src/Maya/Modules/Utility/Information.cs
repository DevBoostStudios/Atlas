using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Maya.Modules.Administration
{
    public class Information : ModuleBase<SocketCommandContext>
    {
        private static string Uptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        private static string HeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();

        [Command("info")]
        [Summary("Display the Bot's current status.")]
        public async Task Info()
        {
            var builder = new EmbedBuilder()
                .WithColor(new Color(0xFF9800))
                .WithAuthor(author =>
                {
                    author
                    .WithName("Maya")
                    .WithUrl("https://github.com/EthanChrisp/Maya")
                    .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/33a1d01fc3af4aa5cdf54c1443d84047.webp"); // To Do: Get Client AvatarUrl
                })
                .WithUrl("https://github.com/EthanChrisp/Maya")
                .WithDescription("Fully-Featured C# Discord Bot")
                .AddInlineField("Author", "<@!132693143173857281>") // To Do: Get Client OwnerID
                .AddInlineField("Library", "[Discord.Net 1.0.1](https://github.com/RogueException/Discord.Net)")
                .AddInlineField("Servers", Context.Client.Guilds.Count)
                .AddInlineField("Uptime", Uptime())
                .AddInlineField("Heap", HeapSize() + "MiB")
                .AddInlineField("Latency", (Context.Client as DiscordSocketClient).Latency + "ms")
                .WithFooter(footer =>
                {
                    footer
                    .WithText(Context.User.ToString() + " | " + DateTime.Now.ToString())
                    .WithIconUrl(Context.User.GetAvatarUrl());
                });
            var embed = builder.Build();
            await ReplyAsync("", false, embed)
                .ConfigureAwait(false);
        }

        [Command("userinfo")]
        [Summary("Return information on the specified User.")]
        public async Task UserInfo(IUser user)
        {
            var builder = new EmbedBuilder()
                .WithColor(new Color(7506394))
                .WithAuthor(author =>
                {
                    author
                    .WithName(user.Username + "#" + user.DiscriminatorValue)
                    .WithIconUrl(user.GetAvatarUrl());
                })
                .AddInlineField("ID", user.Id)
                .AddInlineField("Status", user.Status)
                .AddInlineField("Joined Discord", user.CreatedAt)
                .AddInlineField("Joined Server", "todo") // To Do: Finish UserInfo Logic
                .WithThumbnailUrl(user.GetAvatarUrl())
                .WithFooter(footer =>
                {
                    footer
                    .WithText(Context.User.ToString() + " | " + DateTime.Now.ToString())
                    .WithIconUrl(Context.User.GetAvatarUrl());
                });
            var embed = builder.Build();
            await ReplyAsync("", false, embed)
                .ConfigureAwait(false);
        }

        [RequireOwner]
        [Command("ping")]
        [Summary("Display the Bot's current Round Trip Latency.")]
        public async Task Ping()
        {
            var builder = new EmbedBuilder()
                .WithAuthor(author =>
                {
                    author
                    .WithName("Latency")
                    .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/33a1d01fc3af4aa5cdf54c1443d84047.webp"); // To Do: Get Client AvatarUrl
                })
                .WithColor(new Color(0xFF9800))
                .AddField("RTT", "" + (Context.Client as DiscordSocketClient).Latency + "ms")
                .WithFooter(footer =>
                {
                    footer
                    .WithText(Context.User.ToString() + " | " + DateTime.Now.ToString())
                    .WithIconUrl(Context.User.GetAvatarUrl());
                });
            var embed = builder.Build();
            await ReplyAsync("", false, embed)
                .ConfigureAwait(false);
        }
    }
}