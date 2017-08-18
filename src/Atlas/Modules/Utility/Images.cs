using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Atlas.Modules.Utility
{
    public class Images : ModuleBase<SocketCommandContext>
    {
        private IConfiguration _config;

        [Command("cat")]
        [Summary("Returns a random Cat image.")]
        public async Task Cat()
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync("http://random.cat/meow");
                dynamic parse = JsonConvert.DeserializeObject(json);
                string result = parse.file;

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Cat")
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                    })
                    .WithUrl(result)
                    .WithImageUrl(result)
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

        [Command("dog")]
        [Summary("Returns a random Dog image.")]
        public async Task Dog()
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync("https://dog.ceo/api/breeds/image/random");
                dynamic parse = JsonConvert.DeserializeObject(json);
                string result = parse.message;

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Dog")
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                    })
                    .WithUrl(result)
                    .WithImageUrl(result)
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

        [Command("bunny")]
        [Summary("Returns a random Bunny image.")]
        public async Task Bunny()
        {
            using (var client = new HttpClient())
            {
                _config = BuildConfig();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", _config["imgurClientID"]);
                var json = await client.GetStringAsync("https://api.imgur.com/3/gallery/r/Rabbits/top/month");
                dynamic parse = JsonConvert.DeserializeObject(json);
                Random rnd = new Random();
                int image = rnd.Next(0, 100);
                string result = parse.data[image].link;

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Bunny")
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/4655f79a722eb1e0ec4afc61b2a756a6.webp"); // To Do: Get Client AvatarUrl
                    })
                    .WithUrl(result)
                    .WithImageUrl(result)
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

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
        }
    }
}
