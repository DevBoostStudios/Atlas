using Discord;
using Discord.Commands;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Maya.Modules.Administration
{
    public class Administration : ModuleBase<SocketCommandContext>
    {
        private static string Uptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");

        [RequireOwner]
        [Command("shutdown")]
        [Summary("Shutdown the current Bot instance.")]
        public async Task Shutdown()
        {
            var builder = new EmbedBuilder()
                .WithColor(new Color(0xFF9800))
                .WithAuthor(author =>
                {
                    author
                    .WithName("Shutting Down...")
                    .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/33a1d01fc3af4aa5cdf54c1443d84047.webp"); // To Do: Get Client AvatarUrl
                })
                .AddField("Uptime", Uptime())
                .WithFooter(footer =>
                {
                    footer
                    .WithText(Context.User.ToString() + " | " + DateTime.Now.ToString())
                    .WithIconUrl(Context.User.GetAvatarUrl());
                });
            var embed = builder.Build();
            await ReplyAsync("", false, embed)
                .ConfigureAwait(false);

            await Context.Client.SetStatusAsync(UserStatus.Invisible);
            await Task.Delay(1000);
            Environment.Exit(0);
        }

        [RequireOwner]
        [Command("restart")]
        [Summary("Shutdown the current Bot instance and begin a new.")]
        public async Task Restart()
        {
            var builder = new EmbedBuilder()
                .WithColor(new Color(0xFF9800))
                .WithAuthor("Restarting...")
                .WithFooter(footer =>
                {
                    footer
                    .WithText(Context.User.ToString() + " | " + DateTime.Now.ToString())
                    .WithIconUrl(Context.User.GetAvatarUrl());
                });
            var embed = builder.Build();
            await ReplyAsync("", false, embed)
                .ConfigureAwait(false);

            // To Do: Restart logic
        }
    }
}
