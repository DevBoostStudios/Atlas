using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Atlas.Modules.Administration
{
    public class Information : ModuleBase<SocketCommandContext>
    {
        private IConfiguration _config;
        private static string Uptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        private static string HeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();

        [Command("info")]
        [Summary("Display the Bot's current status.")]
        public async Task Info()
        {
            _config = BuildConfig();

            var builder = new EmbedBuilder()
                .WithColor(new Color(0xFF9800))
                .WithAuthor(author =>
                {
                    author
                    .WithName("Atlas")
                    .WithUrl("https://github.com/EthanChrisp/Atlas")
                    .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/33a1d01fc3af4aa5cdf54c1443d84047.webp"); // To Do: Get Client AvatarUrl
                })
                .WithUrl("https://github.com/EthanChrisp/Atlas")
                .WithDescription("Music, Utility, and Game Discord Bot")
                .AddInlineField("Author", "<@!" + _config["ownerID"] + ">")
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
                .AddField("RTT", (Context.Client as DiscordSocketClient).Latency + "ms")
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

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
        }
    }
}