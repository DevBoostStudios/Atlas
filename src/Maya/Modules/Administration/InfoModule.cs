using System;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Maya.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
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
                    .WithUrl("https://discordapp.com")
                    .WithIconUrl("https://cdn.discordapp.com/embed/avatars/0.png");
                })
                .WithUrl("https://github.com/EthanChrisp/Maya")
                .WithDescription("Fully-Featured C# Discord Bot")
                .AddInlineField("Author", "@LackingAGoodName#4444")
                .AddInlineField("Servers", "" + Context.Client.Guilds.Count + "")
                .AddInlineField("Users", "456")
                .AddInlineField("Uptime", "" + Uptime() + "")
                .AddInlineField("Heap", "" + HeapSize() + "Mb")
                .AddInlineField("Library", "[Discord.Net 1.0.1](https://github.com/RogueException/Discord.Net)")
                .AddInlineField("API", "v6")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Dank")
                    .WithIconUrl("https://cdn.discordapp.com/embed/avatars/0.png");
                });
            var embed = builder.Build();
            await Context.Channel.SendMessageAsync("", false, embed)
                .ConfigureAwait(false);
        }

        [Command("ping")]
        [Summary("Display the Bot's current Round Trip Latency.")]
        public async Task Ping()
        {
            var builder = new EmbedBuilder()
                .WithAuthor("Maya")
                .WithColor(new Color(0xFF9800))
                .AddField("Latency", "" + (Context.Client as DiscordSocketClient).Latency + "ms")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("Dank")
                    .WithIconUrl("https://cdn.discordapp.com/embed/avatars/0.png");
                });
            var embed = builder.Build();
            await Context.Channel.SendMessageAsync("", false, embed)
                        .ConfigureAwait(false);
        }
    }
}