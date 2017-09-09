using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
                    .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                })
                .WithUrl("https://github.com/EthanChrisp/Atlas")
                .WithDescription("Music, Utility, and Game Discord Bot")
                .AddInlineField("Author", "<@!" + _config["ownerID"] + ">")
                .AddInlineField("Library", "[Discord.Net 1.0.2](https://github.com/RogueException/Discord.Net)")
                .AddInlineField("Servers", Context.Client.Guilds.Count)
                .AddInlineField("Uptime", Uptime())
                .AddInlineField("Heap", HeapSize() + "MiB")
                .AddInlineField("Latency", Context.Client.Latency + "ms")
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
        [Command("ping", RunMode = RunMode.Async)]
        [Summary("Returns the current estimated Round-Trip Latency over WebSocket.")]
        public async Task Ping()
        {
            ulong target = 0;
            CancellationTokenSource cts = new CancellationTokenSource();

            Task WaitTarget(SocketMessage msg)
            {
                if (msg.Id != target) return Task.CompletedTask;
                cts.Cancel();
                return Task.CompletedTask;
            }

            var sw = Stopwatch.StartNew();
            var message = await ReplyAsync("Init ---, RTT ---");
            var init = sw.ElapsedMilliseconds;
            target = message.Id;
            sw.Restart();
            Context.Client.MessageReceived += WaitTarget;

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(30), cts.Token);
            }

            catch (TaskCanceledException)
            {
                var rtt = sw.ElapsedMilliseconds;
                sw.Stop();
                await message.ModifyAsync(x => x.Content = "Init " + init + "ms, RTT " + rtt + "ms");
                return;
            }

            finally
            {
                Context.Client.MessageReceived -= WaitTarget;
            }

            sw.Stop();
            await message.ModifyAsync(x => x.Content = "Init " + init + "ms, RTT Timeout");

            // To Do: Complete this shit

            var builder = new EmbedBuilder()
                .WithAuthor(author =>
                {
                    author
                    .WithName("Latency")
                    .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                })
                .WithColor(new Color(0xFF9800))
                .AddInlineField("Heartbeat", Context.Client.Latency + "ms")
                .AddInlineField("Init", "TBDms")
                .AddInlineField("RTT", "TBDms")
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