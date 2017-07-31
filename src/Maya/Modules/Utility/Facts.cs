using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Maya.Modules.Utility
{
    public class Facts : ModuleBase<SocketCommandContext>
    {
        [Command("chucknorris")]
        [Summary("Returns a random fact about Chuck Norris.")]
        public async Task chucknorris()
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync("https://api.chucknorris.io/jokes/random");
                dynamic parse = JsonConvert.DeserializeObject(json);
                string result = parse.value;

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Chuck Norris")
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/33a1d01fc3af4aa5cdf54c1443d84047.webp"); // To Do: Get Client AvatarUrl
                    })
                    .AddInlineField("Fact", result)
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

        [Command("catfact")]
        [Summary("Returns a random fact about cats.")]
        public async Task catfact()
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync("http://www.catfact.info/api/v1/facts.json?per_page=688");
                dynamic parse = JsonConvert.DeserializeObject(json);
                Random rnd = new Random();
                int id = rnd.Next(0, 688);
                string result = parse.facts[id].details;
                string number = parse.facts[id].id;

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Cat Fact")
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/33a1d01fc3af4aa5cdf54c1443d84047.webp"); // To Do: Get Client AvatarUrl
                    })
                    .AddInlineField("#" + number, result)
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
