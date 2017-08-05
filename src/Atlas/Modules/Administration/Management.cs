﻿using Discord;
using Discord.Commands;
// using Atlas.Modules.Audio;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Atlas.Modules.Administration
{
    public class Management : ModuleBase<SocketCommandContext>
    {
        private static string Uptime() => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        // private readonly AudioService _service;

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

            // await _service.LeaveAudio(Context.Guild); // To Do: LeaveAudio on all Guilds
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

        [Group("set")]
        [Summary("Set Bot options and attributes.")]
        public class set : ModuleBase<SocketCommandContext>
        {
            [RequireOwner]
            [Command("name")]
            [Summary("Set the Bot username.")]
            public async Task SetName()
            {
                // To Do: Set Name logic
            }

            [RequireOwner]
            [Command("avatar")]
            [Summary("Set the Bot avatar.")]
            public async Task SetAvatar()
            {
                // To Do: Set Avatar logic
            }
        }
    }
}