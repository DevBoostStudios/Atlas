using Discord.Commands;
using System.Threading.Tasks;

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
    }
}