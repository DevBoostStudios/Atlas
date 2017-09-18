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
    public class Genius : ModuleBase<ICommandContext>
    {
        private IConfiguration _config;

        [Command("song", RunMode = RunMode.Async)]
        [Summary("Returns detailed information on the specified Song via Genius.")]
        public async Task Song([Remainder] string song)
        {
            using (Context.Channel.EnterTypingState())
            {
                using (var client = new HttpClient())
                {
                    _config = BuildConfig();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config["geniusToken"]);
                    var searchJSON = await client.GetStringAsync("https://api.genius.com/search?q=" + song);
                    dynamic searchParse = JsonConvert.DeserializeObject(searchJSON);

                    string songID = searchParse.response.hits[0].result.id;

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config["geniusToken"]);
                    var json = await client.GetStringAsync("https://api.genius.com/songs/" + songID);
                    dynamic parse = JsonConvert.DeserializeObject(json);

                    string title = parse.response.song.full_title;
                    string url = parse.response.song.url;
                    string thumbURL = parse.response.song.song_art_image_thumbnail_url;
                    string description = parse.response.song.description.dom.children[0].children[0];

                    string dateInit = parse.response.song.release_date;
                    string dateYear = dateInit.Split('-')[0];
                    string dateMonth = dateInit.Split('-')[1];
                    string dateDay = dateInit.Split('-')[2];
                    DateTime date = new DateTime(Convert.ToInt32(dateYear), Convert.ToInt32(dateMonth), Convert.ToInt32(dateDay));
                    string release = date.ToString("MMMM dd, yyyy");

                    string album = parse.response.song.album.name;
                    string albumURL = parse.response.song.album.url;

                    var builder = new EmbedBuilder()
                        .WithColor(new Color(5025616))
                        .WithAuthor(author =>
                        {
                            author
                            .WithName(title)
                            .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/e1bd70b2b7cddfc60a15f9cb22403ccb.webp"); // To Do: Get Client AvatarUrl
                        })
                        .WithUrl(url)
                        .WithThumbnailUrl(thumbURL)
                        .WithDescription(description + "…")
                        .AddInlineField("Release", release)
                        .AddInlineField("Album", "[" + album + "](" + albumURL + ")")
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

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
        }
    }
}
