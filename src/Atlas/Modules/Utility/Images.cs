using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/e1bd70b2b7cddfc60a15f9cb22403ccb.webp"); // To Do: Get Client AvatarUrl
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
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/e1bd70b2b7cddfc60a15f9cb22403ccb.webp"); // To Do: Get Client AvatarUrl
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
                        .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/e1bd70b2b7cddfc60a15f9cb22403ccb.webp"); // To Do: Get Client AvatarUrl
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

        [Command("meme")]
        [Summary("Generates a Meme using the specified Image and Text. Seperate Top and Bottom Text with |.")]
        public async Task Meme(string imageURL, [Remainder] string memeTextInit)
        {
            var memeText = memeTextInit.Replace(" ", "_");
            var topTextInit = memeText.Split('|')[0];
            var bottomTextInit = memeText.Split('|')[1];
            var topText = topTextInit.Replace("|", "");
            var bottomText = bottomTextInit.Replace("|", "");

            var memeURL = "https://memegen.link/custom/" + topText + "/" + bottomText + ".jpg?alt=" + imageURL;

            var builder = new EmbedBuilder()
                .WithColor(new Color(5025616))
                .WithAuthor(author =>
                {
                    author
                    .WithName("Meme")
                    .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/e1bd70b2b7cddfc60a15f9cb22403ccb.webp"); // To Do: Get Client AvatarUrl
                })
                .WithImageUrl(memeURL)
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
