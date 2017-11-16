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
    public class R6Siege : ModuleBase<SocketCommandContext>
    {
        private IConfiguration _config;

        [Command("r6siege", RunMode = RunMode.Async)]
        [Summary("Retrieve detailed statistics for the specified Rainbow 6 Siege player.")]
        public async Task R6SiegeStats(string platform, [Remainder] string username)
        {
            using (Context.Channel.EnterTypingState())
            {
                using (var client = new HttpClient())
                {
                    _config = BuildConfig();

                    client.DefaultRequestHeaders.Add("x-app-id", _config["r6dbKey"]);
                    var json = await client.GetStringAsync("https://r6db.com/api/v2/players?name=" + username + "&platform=" + platform); // To Do: Sanitize Platforms and Usernames if necessary (PS4 -> PSN, # -> -, etc)
                    dynamic parse = JsonConvert.DeserializeObject(json);

                    string playerID = parse[0].id;

                    var statsJSON = await client.GetStringAsync("https://r6db.com/api/v2/players/" + playerID);
                    dynamic statsParse = JsonConvert.DeserializeObject(statsJSON);

                    string playername = statsParse.name;
                    string playerPlatform = statsParse.platform;
                    string flair = (statsParse.flair == null) ? "https://i.imgur.com/4fVUY7z.jpg" : statsParse.flair;
                    string lastUpdated = statsParse.updated_at; // To Do: Make this pretty
                    string level = statsParse.level;
                    string matchesPlayed = string.Format("{0:n0}", statsParse.stats.general.played);

                    int playTime = statsParse.stats.general.timePlayed;
                    TimeSpan timePlayed = TimeSpan.FromSeconds(playTime);
                    string played = string.Format("{0:D2} Days {1:D2} Hours",
                        timePlayed.Days,
                        timePlayed.Hours);

                    double kills = statsParse.stats.general.kills;
                    double deaths = statsParse.stats.general.deaths;
                    double kdr = kills / deaths;
                    double kdRatio = Math.Round(kdr, 2);

                    double won = statsParse.stats.general.won;
                    double lost = statsParse.stats.general.lost;
                    double wlr = won / lost;
                    double wlRatio = Math.Round(wlr, 2);

                    double shotsFired = statsParse.stats.general.bulletsFired;
                    double shotsHit = statsParse.stats.general.bulletsHit;
                    double firedHitPerc = (shotsHit / shotsFired) * 100;
                    double accuracy = Math.Round(firedHitPerc, 0);

                    string headshots = string.Format("{0:n0}", statsParse.stats.general.headshot);
                    string assists = string.Format("{0:n0}", statsParse.stats.general.assists);
                    string revives = string.Format("{0:n0}", statsParse.stats.general.revives);
                    string serversHacked = string.Format("{0:n0}", statsParse.stats.general.serversHacked);
                    string hostageResuces = string.Format("{0:n0}", statsParse.stats.general.hostageRescue);
                    string gadgetsDestroyed = string.Format("{0:n0}", statsParse.stats.general.gadgetsDestroyed);

                    string rankedSeason = statsParse.rank.season;
                    string rank = (statsParse.rank.ncsa.rank == "0") ? "Unranked" : statsParse.rank.ncsa.rank; // To Do: Find a way to specify Region (NCSA, EMEA, APAC), also convert Rank Number to Rank Name (18 = Plat II)
                    string mmr = string.Format("{0:n0}", statsParse.rank.ncsa.mmr);
                    string globalRank = (statsParse.placements.global == null) ? "N/A" : string.Format("{0:n0}", statsParse.placements.global);

                    double rankedWon = statsParse.rank.ncsa.wins;
                    double rankedLost = statsParse.rank.ncsa.losses;
                    double rankedWLR = rankedWon / rankedLost; // To Do: Figure out what to do if 0 / 0 (NaN)
                    double rankedWLRatio = Math.Round(rankedWLR, 2);

                    var builder = new EmbedBuilder()
                        .WithColor(new Color(5025616))
                        .WithAuthor(author =>
                        {
                            author
                            .WithName("Rainbow 6 Siege - " + playername + " (" + playerPlatform + ")")
                            .WithIconUrl(flair);
                        })
                        .WithUrl("https://r6db.com/player/" + playerID)
                        .WithDescription("Updated: " + lastUpdated)
                        .AddInlineField("Level", level)
                        .AddInlineField("Matches", matchesPlayed)
                        .AddInlineField("Time Played", played)
                        .AddInlineField("Kills", string.Format("{0:n0}", kills))
                        .AddInlineField("Deaths", string.Format("{0:n0}", deaths))
                        .AddInlineField("K/D Ratio", kdRatio.ToString())
                        .AddInlineField("Wins", string.Format("{0:n0}", won))
                        .AddInlineField("Losses", string.Format("{0:n0}", lost))
                        .AddInlineField("W/L Ratio", wlRatio)
                        .AddInlineField("Shots Fired", string.Format("{0:n0}", shotsFired))
                        .AddInlineField("Shots Hit", string.Format("{0:n0}", shotsHit))
                        .AddInlineField("Accuracy", accuracy + "%")
                        .AddInlineField("Headshots", headshots)
                        .AddInlineField("Assists", assists)
                        .AddInlineField("Revives", revives)
                        .AddInlineField("Servers Hacked", serversHacked)
                        .AddInlineField("Hostage Rescues", hostageResuces)
                        .AddInlineField("Gadgets Destroyed", gadgetsDestroyed)
                        .AddField("​", "__**Ranked Season " + rankedSeason + "**__")
                        .AddInlineField("Rank", rank)
                        .AddInlineField("Global Rank", globalRank)
                        .AddInlineField("MMR", mmr)
                        .AddInlineField("Won", string.Format("{0:n0}", rankedWon))
                        .AddInlineField("Lost", string.Format("{0:n0}", rankedLost))
                        .AddInlineField("W/L Ratio", rankedWLRatio)
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
