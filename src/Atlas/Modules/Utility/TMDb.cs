using Discord;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Atlas.Modules.Utility
{
    public class TMDb : ModuleBase<SocketCommandContext>
    {
        private IConfiguration _config;

        [Command("movie")]
        [Summary("Returns detailed information from TMDb on the specified Movie.")]
        public async Task Movie([Remainder] string title)
        {
            using (Context.Channel.EnterTypingState())
            {
                using (var client = new HttpClient())
                {
                    _config = BuildConfig();

                    var json = await client.GetStringAsync("https://api.themoviedb.org/3/search/movie?api_key=" + _config["tmdbKey"] + "&query=" + title);
                    dynamic parse = JsonConvert.DeserializeObject(json);

                    string tmdbID = parse.results[0].id;

                    var detailsJSON = await client.GetStringAsync("https://api.themoviedb.org/3/movie/" + tmdbID + "?api_key=" + _config["tmdbKey"]);
                    dynamic detailsParse = JsonConvert.DeserializeObject(detailsJSON);

                    string movieTitle = parse.results[0].title;
                    string imdbID = detailsParse.imdb_id;

                    string overview = parse.results[0].overview;

                    string poster = "https://image.tmdb.org/t/p/w640" + parse.results[0].poster_path;

                    string dateInit = parse.results[0].release_date;
                    string dateYear = dateInit.Split('-')[0];
                    string dateMonth = dateInit.Split('-')[1];
                    string dateDay = dateInit.Split('-')[2];
                    DateTime date = new DateTime(Convert.ToInt32(dateYear), Convert.ToInt32(dateMonth), Convert.ToInt32(dateDay));
                    string release = date.ToString("MMMM dd, yyyy");

                    string genre = detailsParse.genres[0].name;
                    string rating = parse.results[0].vote_average + "/10";

                    double runtime = detailsParse.runtime;
                    TimeSpan time = TimeSpan.FromMinutes(runtime);
                    string length = time.ToString();

                    string budget = "$" + string.Format("{0:n0}", detailsParse.budget);
                    string revenue = "$" + string.Format("{0:n0}", detailsParse.revenue);

                    var builder = new EmbedBuilder()
                        .WithColor(new Color(5025616))
                        .WithAuthor(author =>
                        {
                            author
                            .WithName(movieTitle)
                            .WithUrl("http://www.imdb.com/title/" + imdbID)
                            .WithIconUrl("http://i.imgur.com/YuiDMzL.png");
                        })
                        .WithDescription(overview)
                        .WithThumbnailUrl(poster)
                        .AddInlineField("Release", release)
                        .AddInlineField("Genre", genre)
                        .AddInlineField("Rating", rating)
                        .AddInlineField("Length", length)
                        .AddInlineField("Budget", budget)
                        .AddInlineField("Revenue", revenue)
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

        [Command("actor")]
        [Summary("Returns detailed information from TMDb on the specified Actor/Actress.")]
        public async Task Actor([Remainder] string actor)
        {
            using (Context.Channel.EnterTypingState())
            {
                using (var client = new HttpClient())
                {
                    _config = BuildConfig();

                    var json = await client.GetStringAsync("https://api.themoviedb.org/3/search/person?api_key=" + _config["tmdbKey"] + "&query=" + actor);
                    dynamic parse = JsonConvert.DeserializeObject(json);

                    string tmdbID = parse.results[0].id;

                    var detailsJSON = await client.GetStringAsync("https://api.themoviedb.org/3/person/" + tmdbID + "?api_key=" + _config["tmdbKey"]);
                    dynamic detailsParse = JsonConvert.DeserializeObject(detailsJSON);

                    string name = parse.results[0].name;
                    string imdbID = detailsParse.imdb_id;

                    string overviewInit = detailsParse.biography;
                    string overview = Ellipsis(overviewInit, 250);

                    string portrait = "https://image.tmdb.org/t/p/w640" + detailsParse.profile_path;

                    string bornDateInit = detailsParse.birthday;
                    string bornDateYear = bornDateInit.Split('-')[0];
                    string bornDateMonth = bornDateInit.Split('-')[1];
                    string bornDateDay = bornDateInit.Split('-')[2];
                    DateTime bornDate = new DateTime(Convert.ToInt32(bornDateYear), Convert.ToInt32(bornDateMonth), Convert.ToInt32(bornDateDay));
                    string born = bornDate.ToString("MMMM dd, yyyy");

                    string birthplace = detailsParse.place_of_birth;


                    var builder = new EmbedBuilder()
                        .WithColor(new Color(5025616))
                        .WithAuthor(author =>
                        {
                            author
                            .WithName(name)
                            .WithUrl("http://www.imdb.com/name/" + imdbID)
                            .WithIconUrl("http://i.imgur.com/YuiDMzL.png");
                        })
                        .WithDescription(overview)
                        .WithThumbnailUrl(portrait)
                        .AddInlineField("Born", born)
                        .AddInlineField("Birthplace", birthplace)
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

        public string Ellipsis(string text, int length)
        {
            return text.Substring(0, length) + "…";
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
