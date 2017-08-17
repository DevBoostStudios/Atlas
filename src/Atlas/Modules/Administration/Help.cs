using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord;

namespace Atlas.Modules.Administration
{
    public class Help : ModuleBase<ICommandContext>
    {
        private CommandService _service;
        Help(CommandService Service)
        {
            _service = Service;
        }

        [Command("help")]
        [Summary("Returns a list of Commands.")]
        public async Task HelpList()
        {
            // To Do: Help Command logic
            await ReplyAsync("To Do", false);
        }

        [Command("help")]
        [Summary("Returns detailed information on the specified Command.")]
        public async Task HelpCmd(string commandName)
        {
            var result = _service.Search(Context, commandName);

            foreach (var command in result.Commands)
            {
                var cmd = command.Command;

                var builder = new EmbedBuilder()
                    .WithColor(new Color(0xFF9800))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Help - " + commandName)
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                })
                    .WithDescription(cmd.Summary)
                    .AddInlineField("Parameters", string.Join(", ", cmd.Parameters))
                    .AddInlineField("Remarks", cmd.Remarks)
                    // To Do: This is broken still
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

            //if (!result.IsSuccess)
            //{
            //    await ReplyAsync("", false, embed);
            //    return;
            //}

            //embed.Description = "";

            //foreach (var command in result.Commands)
            //{
            //    var cmd = command.Command;
            //    if (cmd.Parameters == null)
            //        embed.AddInlineField("Parameters: ", "None");
            //    else
            //        embed.AddInlineField("Parameters: ", string.Join(", ", cmd.Parameters));

            //    if (cmd.Summary == null)
            //        embed.AddInlineField("Summary: ", "None");
            //    else
            //        embed.AddInlineField("Summary: ", cmd.Summary);

            //    if (cmd.Remarks == null)
            //        embed.AddInlineField("Remarks: ", "None");
            //    else
            //        embed.AddInlineField("Remarks: ", cmd.Remarks);

            //    await ReplyAsync("", false, embed);
            //}
        }
    }
}