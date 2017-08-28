using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Atlas.Modules.Utility
{
    public class Destiny : ModuleBase<SocketCommandContext>
    {
        // Endpoints?: https://gist.github.com/jdstein1/76efaf11e664133f3ca8

        [Group("Destiny")]
        [Summary("To Do")]
        public class DestinyCmd : ModuleBase<SocketCommandContext>
        {
            private IConfiguration _config;

            [Command("stats")]
            [Summary("To Do")]
            public async Task DestinyStats(string platform, [Remainder] string username)
            {
                using (Context.Channel.EnterTypingState())
                {
                    using (var client = new HttpClient())
                    {
                        _config = BuildConfig();

                        client.DefaultRequestHeaders.Add("X-API-Key", _config["bungieKey"]);
                        var json = await client.GetStringAsync("http://www.bungie.net/Platform/User/GetBungieNetUserById/8310647/?lc=en&fmt=true&lcin=true");
                        dynamic parse = JsonConvert.DeserializeObject(json);

                        // To Do: Finish this
                    }
                }
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
}
