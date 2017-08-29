using Discord;
using Discord.Commands;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Atlas.Modules.Administration
{
    public class Management : ModuleBase<SocketCommandContext>
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
                    .WithName("Atlas")
                    .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                })
                .WithDescription("Shutting down...")
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

            // To Do: LeaveVoice on all Guilds
            await Context.Client.SetStatusAsync(UserStatus.Invisible);
            await Task.Delay(500);
            Environment.Exit(0);
        }

        [RequireOwner]
        [Command("restart")]
        [Summary("Shutdown the current Bot instance and begin a new.")]
        public async Task Restart()
        {
            var builder = new EmbedBuilder()
                .WithColor(new Color(0xFF9800))
                .WithAuthor("Atlas")
                .WithDescription("Restarting...")
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

        [Group("set")]
        [Summary("Set Bot options and attributes.")]
        public class set : ModuleBase<SocketCommandContext>
        {
            [RequireOwner]
            [Command("name")]
            [Summary("Set the Bot username.")]
            public async Task SetName(string username)
            {
                await Context.Client.CurrentUser.ModifyAsync(u => u.Username = username);

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
                    .WithDescription("Username changed to " + username)
                    .WithFooter(footer =>
                    {
                        footer
                        .WithText(Context.User.ToString() + " | " + DateTime.Now.ToString())
                        .WithIconUrl(Context.User.GetAvatarUrl());
                    });
                var embed = builder.Build();
                await ReplyAsync("", false, embed)
                    .ConfigureAwait(false);
                // To Do: This is broken now?????
            }

            [RequireOwner]
            [Command("avatar")]
            [Summary("Set the Bot avatar.")]
            public async Task SetAvatar(string url)
            {
                var imageStream = new MemoryStream();

                using (var client = new HttpClient())
                {
                    using (var stream = await client.GetStreamAsync(url))
                    {
                        await stream.CopyToAsync(imageStream);
                        imageStream.Position = 0;
                    }
                }

                await Context.Client.CurrentUser.ModifyAsync(x => x.Avatar = new Image(imageStream));

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
                    .WithDescription("Avatar updated")
                    .WithImageUrl(url)
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
            [Command("game")]
            [Summary("Set the Bot playing status.")]
            public async Task SetGame([Remainder] string game)
            {
                if (game == "none")
                {
                    await Context.Client.SetGameAsync(null);

                    var builder = new EmbedBuilder()
                        .WithColor(new Color(0xFF9800))
                        .WithAuthor(author =>
                        {
                            author
                            .WithName("Atlas")
                            .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                    })
                        .WithDescription("Cleared Playing status.")
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

                else
                {
                    await Context.Client.SetGameAsync(game);

                    var builder = new EmbedBuilder()
                        .WithColor(new Color(0xFF9800))
                        .WithAuthor(author =>
                        {
                            author
                            .WithName("Atlas")
                            .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                        })
                        .WithDescription("Set Playing status to " + game + ".")
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

            [RequireOwner]
            [Command("online")]
            [Summary("Set the Bot status to Online.")]
            public async Task SetOnline()
            {
                await Context.Client.SetStatusAsync(UserStatus.Online);

                var builder = new EmbedBuilder()
                    .WithColor(new Color(4437377))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Atlas")
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                })
                    .WithDescription("Status set to Online.")
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
            [Command("idle")]
            [Summary("Set the Bot status to Idle.")]
            public async Task SetAway()
            {
                await Context.Client.SetStatusAsync(UserStatus.Idle);

                var builder = new EmbedBuilder()
                    .WithColor(new Color(16426522))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Atlas")
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                    })
                    .WithDescription("Status set to Idle.")
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
            [Command("disturb")]
            [Summary("Set the Bot status to Do Not Disturb.")]
            public async Task SetDoNotDisturb()
            {
                await Context.Client.SetStatusAsync(UserStatus.DoNotDisturb);

                var builder = new EmbedBuilder()
                    .WithColor(new Color(15746887))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Atlas")
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                    })
                    .WithDescription("Status set to Do Not Disturb.")
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
            [Command("invisible")]
            [Summary("Set the Bot status to Invisible.")]
            public async Task SetInvisible()
            {
                await Context.Client.SetStatusAsync(UserStatus.Invisible);

                var builder = new EmbedBuilder()
                    .WithColor(new Color(7634877))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Atlas")
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                    })
                    .WithDescription("Status set to Invisible.")
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
}
