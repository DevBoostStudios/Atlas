using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.IO;
using Discord;

namespace Atlas.Modules.Administration
{
    public class Help : ModuleBase<ICommandContext>
    {
        private CommandService _service;
        private IConfiguration _config;
        //public HelpModule(CommandService service)
        //{
        //    _service = service;
        //}

        [Command("help")]
        [Summary("Returns a list of all commands.")]
        public async Task HelpCmd()
        {
            _config = BuildConfig();
            string prefix = _config["prefix"];

            // To Do: Fix this mess
            //foreach (var module in _service.Modules)
            //{
            //    foreach (var cmd in module.Commands)
            //    {
            //        var result = await cmd.CheckPreconditionsAsync(Context);
            //        if (result.IsSuccess)
            //            description += prefix + cmd.Aliases.First() + "\n";
            //    }

            //    if (!string.IsNullOrWhiteSpace(description))
            //    {
            //        builder.AddField(module.Name, description);
            //    };
            //}

            var builder = new EmbedBuilder()
                .WithColor(new Color(0xFF9800))
                .WithAuthor(author =>
                {
                    author
                    .WithName("Help")
                    .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                })
                .WithDescription(prefix)
                //.AddField(module.name, description)
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

        [Command("help")]
        [Summary("Returns detailed information on the specified command.")]
        public async Task HelpCmd(string command)
        {
            // To Do: Help Command logic
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
