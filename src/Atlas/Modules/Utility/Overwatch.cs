using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Atlas.Modules.Utility
{
    public class Overwatch : ModuleBase<SocketCommandContext>
    {
        [Group("Overwatch")]
        [Summary("Retrieve information for the specified Overwatch player.")]
        public class OW : ModuleBase<SocketCommandContext>
        {
            [Command("stats")]
            [Summary("Retrieve statistics for the specified Overwatch player.")]
            public async Task OWStats(string platform, string username)
            {
                using (Context.Channel.EnterTypingState())
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36");
                        var json = await client.GetStringAsync("https://owapi.net/api/v3/u/" + username + "/blob?platform=" + platform);
                        dynamic parse = JsonConvert.DeserializeObject(json);

                        string portrait = parse.any.stats.quickplay.overall_stats.avatar;
                        string skillRating = parse.any.stats.competitive.overall_stats.comprank;

                        string skillTierInit = parse.any.stats.competitive.overall_stats.tier;
                        string skillTier = skillTierInit.Substring(0, 1).ToUpper() + skillTierInit.Substring(1);

                        string prestige = parse.any.stats.quickplay.overall_stats.prestige * 100;
                        string rank = parse.any.stats.quickplay.overall_stats.level;
                        int level = int.Parse(prestige) + int.Parse(rank);

                        string qpPlaytime = parse.any.stats.quickplay.game_stats.time_played + " hours";
                        string qpElims = string.Format("{0:n0}", parse.any.stats.quickplay.game_stats.eliminations);
                        string qpDeaths = string.Format("{0:n0}", parse.any.stats.quickplay.game_stats.deaths);
                        string qpWins = parse.any.stats.quickplay.game_stats.games_won;
                        string qpLosses = "-";
                        string qpStreak = parse.any.stats.quickplay.game_stats.kill_streak_best;
                        string qpDamage = string.Format("{0:n0}", parse.any.stats.quickplay.game_stats.all_damage_done);
                        string qpMedals = string.Format("{0:n0}", parse.any.stats.quickplay.game_stats.medals);
                        string qpHealing = string.Format("{0:n0}", parse.any.stats.quickplay.game_stats.healing_done) + "HP";

                        string compPlaytime = parse.any.stats.competitive.game_stats.time_played + " hours";
                        string compElims = string.Format("{0:n0}", parse.any.stats.competitive.game_stats.eliminations);
                        string compDeaths = string.Format("{0:n0}", parse.any.stats.competitive.game_stats.deaths);
                        string compWins = parse.any.stats.competitive.game_stats.games_won;
                        string compLosses = parse.any.stats.competitive.game_stats.games_lost;
                        string compStreak = parse.any.stats.competitive.game_stats.kill_streak_best;
                        string compDamage = string.Format("{0:n0}", parse.any.stats.competitive.game_stats.all_damage_done);
                        string compMedals = string.Format("{0:n0}", parse.any.stats.competitive.game_stats.medals);
                        string compHealing = string.Format("{0:n0}", parse.any.stats.competitive.game_stats.healing_done) + "HP";

                        var builder = new EmbedBuilder()
                            .WithColor(new Color(5025616))
                            .WithAuthor(author =>
                            {
                                author
                                .WithName(username + " (" + platform.ToUpper() + ")")
                                .WithIconUrl(portrait)
                                .WithUrl("https://masteroverwatch.com/profile/" + platform + "/global/" + username);
                            })
                            .AddInlineField("Level", level.ToString())
                            .AddInlineField("Skill Rating", skillRating)
                            .AddInlineField("Skill Tier", skillTier)
                            .AddField("​", "__**Quickplay**__")
                            .AddInlineField("Playtime", qpPlaytime)
                            .AddInlineField("Eliminations", qpElims)
                            .AddInlineField("Deaths", qpDeaths)
                            .AddInlineField("Wins", qpWins)
                            .AddInlineField("Losses", qpLosses)
                            .AddInlineField("Kill Streak", qpStreak)
                            .AddInlineField("Damage", qpDamage)
                            .AddInlineField("Medals", qpMedals)
                            .AddInlineField("Healing", qpHealing)
                            .AddField("​", "__**Current Competitive Season**__")
                            .AddInlineField("Playtime", compPlaytime)
                            .AddInlineField("Eliminations", compElims)
                            .AddInlineField("Deaths", compDeaths)
                            .AddInlineField("Wins", compWins)
                            .AddInlineField("Losses", compLosses)
                            .AddInlineField("Kill Streak", compStreak)
                            .AddInlineField("Damage", compDamage)
                            .AddInlineField("Medals", compMedals)
                            .AddInlineField("Healing", compHealing)
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

            [Command("heroes")]
            [Summary("Retrieve Hero-specific statistics for the specified Overwatch player.")]
            public async Task OWHeroes(string platform, string username)
            {
                using (Context.Channel.EnterTypingState())
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36");
                        var json = await client.GetStringAsync("https://owapi.net/api/v3/u/" + username + "/blob?platform=" + platform);
                        dynamic parse = JsonConvert.DeserializeObject(json);

                        // To Do: OW Heroes logic
                    }
                }
            }
        }
    }
}