using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Atlas.Modules.Utility
{
    public class CallofDuty : ModuleBase<SocketCommandContext>
    {
        [Group("CWL")]
        [Summary("Retrieve information for Call of Duty World League.")]
        public class CWL : ModuleBase<SocketCommandContext>
        {
            [Command("events")]
            [Summary("Retrieve the upcoming Call of Duty World League event.")]
            public async Task CWLEvents()
            {
                using (Context.Channel.EnterTypingState())
                {
                    using (var client = new HttpClient())
                    {
                        var json = await client.GetStringAsync("https://www.callofduty.com/esports/data/v1/content/event-pages/listings");
                        dynamic parse = JsonConvert.DeserializeObject(json); // To Do: Catch and report to user if no upcoming events are available

                        string cwlLogo = "https://i.imgur.com/H6L5XB6.png";
                        string eventName = parse.data.data.upcoming[0].title + " - " + parse.data.data.upcoming[0].subtitle;
                        string eventURL = "https://www.callofduty.com" + parse.data.data.upcoming[0].link;
                        string upcomingEvents = "0"; // To Do: Finish this
                        string eventImageURL = "https://www.callofduty.com" + parse.data.data.upcoming[0].logoThumbnail;

                        long startDate = parse.data.data.upcoming[0].startTime / 1000;
                        DateTimeOffset eventStartInit = DateTimeOffset.FromUnixTimeSeconds(startDate).ToLocalTime();
                        string eventStart = eventStartInit.ToString().Split('-')[0];

                        long endDate = parse.data.data.upcoming[0].endTime / 1000;
                        DateTimeOffset eventEndInit = DateTimeOffset.FromUnixTimeSeconds(endDate).ToLocalTime();
                        string eventEnd = eventEndInit.ToString().Split('-')[0];

                        var builder = new EmbedBuilder()
                            .WithAuthor(author =>
                            {
                                author
                                .WithName(eventName)
                                .WithIconUrl(cwlLogo)
                                .WithUrl(eventURL);
                            })
                            .WithColor(new Color(5025616))
                            .WithDescription("See all " + upcomingEvents + " upcoming Call of Duty World League events **[here](https://www.callofduty.com/esports/events)**.")
                            .WithThumbnailUrl(eventImageURL)
                            .AddInlineField("Event Start", eventStart)
                            .AddInlineField("Event End", eventEnd)
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

        [Group("WWII")]
        [Summary("Retrieve information for Call of Duty: WWII players.")]
        public class WWII : ModuleBase<SocketCommandContext>
        {
            [Group("mp")]
            [Summary("Retrieve Multiplayer information for Call of Duty: WWII players.")]
            public class WWIIMP : ModuleBase<SocketCommandContext>
            {
                [Command("stats", RunMode = RunMode.Async)]
                [Summary("Retrieve Multiplayer statistics for the specified Call of Duty: WWII player.")]
                public async Task WWIIMPStats(string platform, [Remainder] string username)
                {
                    using (Context.Channel.EnterTypingState())
                    {
                        using (var client = new HttpClient())
                        {
                            var json = await client.GetStringAsync("https://my.callofduty.com/api/papi-client/crm/cod/v2/title/wwii/platform/" + platform + "/gamer/" + username + "/profile/"); // To Do: Sanitize Platforms and Usernames if necessary (PS4 -> PSN, etc)
                            dynamic parse = JsonConvert.DeserializeObject(json);

                            //long lastUpdated = parse.data.mp.lifetime.all.lastUpdated / 1000;
                            //DateTimeOffset updatedInit = DateTimeOffset.FromUnixTimeSeconds(lastUpdated).ToLocalTime();
                            //string updated = updatedInit.ToString().Split('-')[0];

                            string prestige = parse.data.mp.prestige;
                            string rank = parse.data.mp.level;

                            int playTime = parse.data.mp.lifetime.all.timePlayed;
                            TimeSpan timePlayed = TimeSpan.FromSeconds(playTime);
                            string played = string.Format("{0:D2} Days {1:D2} Hours",
                                timePlayed.Days,
                                timePlayed.Hours);

                            string kills = string.Format("{0:n0}", parse.data.mp.lifetime.all.kills);
                            string deaths = string.Format("{0:n0}", parse.data.mp.lifetime.all.deaths);
                            double kd = parse.data.mp.lifetime.all.kdRatio;
                            double kdr = Math.Round(kd, 2);

                            string wins = string.Format("{0:n0}", parse.data.mp.lifetime.all.wins);
                            string losses = string.Format("{0:n0}", parse.data.mp.lifetime.all.losses);
                            //double wl = parse.data.mp.lifetime.all.winRatio;
                            //double wlr = Math.Round(wl, 2);
                            double wlrWins = Convert.ToDouble(wins);
                            double wlrLosses = Convert.ToDouble(losses);
                            double wlrTempInit = wlrWins / wlrLosses;
                            string wlrTemp = Math.Round(wlrTempInit, 2).ToString();

                            string score = string.Format("{0:n0}", parse.data.mp.lifetime.all.score);
                            double scorePerMin = parse.data.mp.weekly.all.scorePerMinute;
                            double spm = Math.Round(scorePerMin, 0);

                            string matches = string.Format("{0:n0}", parse.data.mp.lifetime.all.matchesPlayed);
                            string rankIcon = (prestige == "0") ? "https://my.callofduty.com/content/dam/atvi/callofduty/mycod/common/player-icons/wwii/level-" + rank + ".png" : "https://my.callofduty.com/content/dam/atvi/callofduty/mycod/common/player-icons/wwii/prestige-" + prestige + ".png";
                            string headshots = parse.data.mp.lifetime.all.headshots;
                            string suicides = parse.data.mp.lifetime.all.suicides;

                            double lifetimeAccuracy = parse.data.mp.lifetime.all.accuracy;
                            double accuracy = Math.Round(lifetimeAccuracy, 2);

                            //string xp = string.Format("{0:n0}", parse.data.mp.lifetime.all.xp);

                            //double boostScore = parse.data.mp.weekly.all.boostingScore;
                            //double booster = Math.Round(boostScore, 2);
                            //string seasonPass = (parse.data.engagement.seasonPass == "0") ? "No" : "Yes";

                            var builder = new EmbedBuilder()
                                .WithColor(new Color(5025616))
                                .WithAuthor(author =>
                                {
                                    author
                                    .WithName(username + " (" + platform.ToUpper() + ")")
                                    .WithIconUrl(rankIcon)
                                    .WithUrl("https://my.callofduty.com/iw/recent?platform=" + platform + "&username=" + username);
                                })
                                //.WithDescription("Updated: " + updated) // As of 11/8 this isn't in API
                                .AddInlineField("Prestige", prestige)
                                .AddInlineField("Rank", rank)
                                .AddInlineField("Time Played", played)
                                .AddInlineField("Kills", kills)
                                .AddInlineField("Deaths", deaths)
                                .AddInlineField("K/D Ratio", kdr.ToString()) // As of 11/8 this is broken in API
                                .AddInlineField("Wins", wins)
                                .AddInlineField("Losses", losses)
                                .AddInlineField("W/L Ratio", wlrTemp) // As of 11/8 this isn't in API
                                .AddInlineField("Score", score)
                                .AddInlineField("Weekly SPM", spm.ToString()) // As of 11/10 lifetime isn't in API
                                .AddInlineField("Matches", matches)
                                .AddInlineField("Headshots", headshots)
                                .AddInlineField("Suicides", suicides)
                                .AddInlineField("Accuracy", accuracy.ToString() + "%")
                                //.AddInlineField("Rank XP", xp) // As of 11/8 this isn't in API
                                //.AddInlineField("Booster?", booster.ToString() + "% Chance") // As of 11/8 this isn't in API
                                //.AddInlineField("Season Pass?", seasonPass) // As of 11/8 this isn't in API
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
        }

        [Group("IW")]
        [Summary("Retrieve information for Call of Duty: Infinite Warfare players.")]
        public class IW : ModuleBase<SocketCommandContext>
        {
            [Group("mp")]
            [Summary("Retrieve Multiplayer information for Call of Duty: Infinite Warfare players.")]
            public class IWMP : ModuleBase<SocketCommandContext>
            {
                [Command("stats", RunMode = RunMode.Async)]
                [Summary("Retrieve Multiplayer statistics for the specified Call of Duty: Infinite Warfare player.")]
                public async Task IWMPStats(string platform, [Remainder] string username)
                {
                    using (Context.Channel.EnterTypingState())
                    {
                        using (var client = new HttpClient())
                        {
                            var json = await client.GetStringAsync("https://my.callofduty.com/api/papi-client/crm/cod/v2/title/iw/platform/" + platform + "/gamer/" + username + "/profile/"); // To Do: Sanitize Platforms and Usernames if necessary (PS4 -> PSN, etc)
                            dynamic parse = JsonConvert.DeserializeObject(json);

                            long lastUpdated = parse.data.mp.lifetime.all.lastUpdated / 1000;
                            DateTimeOffset updatedInit = DateTimeOffset.FromUnixTimeSeconds(lastUpdated).ToLocalTime();
                            string updated = updatedInit.ToString().Split('-')[0];

                            string prestige = parse.data.mp.prestige;
                            string rank = parse.data.mp.level;

                            int playTime = parse.data.mp.lifetime.all.timePlayed;
                            TimeSpan timePlayed = TimeSpan.FromSeconds(playTime);
                            string played = string.Format("{0:D2} Days {1:D2} Hours",
                                timePlayed.Days,
                                timePlayed.Hours);

                            string kills = string.Format("{0:n0}", parse.data.mp.lifetime.all.kills);
                            string deaths = string.Format("{0:n0}", parse.data.mp.lifetime.all.deaths);
                            double kd = parse.data.mp.lifetime.all.kdRatio;
                            double kdr = Math.Round(kd, 2);

                            string wins = string.Format("{0:n0}", parse.data.mp.lifetime.all.wins);
                            string losses = string.Format("{0:n0}", parse.data.mp.lifetime.all.losses);
                            double wl = parse.data.mp.lifetime.all.winRatio;
                            double wlr = Math.Round(wl, 2);

                            string score = string.Format("{0:n0}", parse.data.mp.lifetime.all.score);
                            double scorePerMin = parse.data.mp.lifetime.all.scorePerMinute;
                            double spm = Math.Round(scorePerMin, 0);

                            string matches = string.Format("{0:n0}", parse.data.mp.lifetime.all.matchesPlayed);
                            string rankIcon = (prestige == "0") ? "https://my.callofduty.com/content/dam/atvi/callofduty/mycod/common/player-icons/iw/level-" + rank + ".png" : "https://my.callofduty.com/content/dam/atvi/callofduty/mycod/common/player-icons/iw/prestige-" + prestige + ".png";
                            string headshots = parse.data.mp.lifetime.all.headshots;
                            string suicides = parse.data.mp.lifetime.all.suicides;

                            double weeklyAccuracy = parse.data.mp.weekly.all.accuracy;
                            double accuracy = Math.Round(weeklyAccuracy, 2);

                            string xp = string.Format("{0:n0}", parse.data.mp.lifetime.all.xp);

                            double boostScore = parse.data.mp.weekly.all.boostingScore;
                            double booster = Math.Round(boostScore, 2);
                            string seasonPass = (parse.data.engagement.seasonPass == "0") ? "No" : "Yes";

                            var builder = new EmbedBuilder()
                                .WithAuthor(author =>
                                {
                                    author
                                    .WithName(username + " (" + platform.ToUpper() + ")")
                                    .WithIconUrl(rankIcon)
                                    .WithUrl("https://my.callofduty.com/iw/recent?platform=" + platform + "&username=" + username);
                                })
                            .WithColor(new Color(5025616))
                            .WithDescription("Updated: " + updated)
                            .AddInlineField("Prestige", prestige)
                            .AddInlineField("Rank", rank)
                            .AddInlineField("Time Played", played)
                            .AddInlineField("Kills", kills)
                            .AddInlineField("Deaths", deaths)
                            .AddInlineField("K/D Ratio", kdr.ToString())
                            .AddInlineField("Wins", wins)
                            .AddInlineField("Losses", losses)
                            .AddInlineField("W/L Ratio", wlr.ToString())
                            .AddInlineField("Score", score)
                            .AddInlineField("SPM", spm.ToString())
                            .AddInlineField("Matches", matches)
                            .AddInlineField("Headshots", headshots)
                            .AddInlineField("Suicides", suicides)
                            .AddInlineField("Weekly Accuracy", accuracy.ToString() + "%") // To Do: Wait for COD API to support Lifetime Accuracy, this errors if player hasn't played in a week
                            .AddInlineField("Rank XP", xp)
                            .AddInlineField("Booster?", booster.ToString() + "% Chance")
                            .AddInlineField("Season Pass?", seasonPass)
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

            [Group("zm")]
            [Summary("Retrieve Zombies information for Call of Duty: Infinite Warfare players.")]
            public class IWZM : ModuleBase<SocketCommandContext>
            {
                [Command("stats", RunMode = RunMode.Async)]
                [Summary("Retrieve Zombies statistics for the specified Call of Duty: Infinite Warfare player.")]
                public async Task IWZMStats(string platform, [Remainder] string username)
                {
                    using (Context.Channel.EnterTypingState())
                    {
                        using (var client = new HttpClient())
                        {
                            var json = await client.GetStringAsync("https://my.callofduty.com/api/papi-client/crm/cod/v2/title/iw/platform/" + platform + "/gamer/" + username + "/profile/"); // To Do: Sanitize Platforms and Usernames if necessary (PS4 -> PSN, etc)
                            dynamic parse = JsonConvert.DeserializeObject(json);

                            long lastUpdated = parse.data.zombies.lifetime.all.lastUpdated / 1000;
                            DateTimeOffset updatedInit = DateTimeOffset.FromUnixTimeSeconds(lastUpdated).ToLocalTime();
                            string updated = updatedInit.ToString().Split('-')[0];

                            string kills = string.Format("{0:n0}", parse.data.zombies.lifetime.all.kills);
                            string downs = string.Format("{0:n0}", parse.data.zombies.lifetime.all.downs);
                            string revives = string.Format("{0:n0}", parse.data.zombies.lifetime.all.revives);

                            int playTime = parse.data.zombies.lifetime.all.timePlayed;
                            TimeSpan timePlayed = TimeSpan.FromSeconds(playTime);
                            string played = string.Format("{0:D2} Days {1:D2} Hours",
                                timePlayed.Days,
                                timePlayed.Hours);

                            string matches = string.Format("{0:n0}", parse.data.zombies.lifetime.all.matchesPlayed);
                            string scenes = string.Format("{0:n0}", parse.data.zombies.lifetime.all.wavesSurvived);
                            string headshots = string.Format("{0:n0}", parse.data.zombies.lifetime.all.headshots);
                            string explosiveKills = string.Format("{0:n0}", parse.data.zombies.lifetime.all.explosiveKills);
                            string shotsLanded = string.Format("{0:n0}", parse.data.zombies.lifetime.all.shotsLanded);
                            string score = string.Format("{0:n0}", parse.data.zombies.lifetime.all.score);
                            string doorsOpened = string.Format("{0:n0}", parse.data.zombies.lifetime.all.doorsOpened);
                            string seasonPass = (parse.data.engagement.seasonPass == "0") ? "No" : "Yes";

                            var builder = new EmbedBuilder()
                                .WithAuthor(author =>
                                {
                                    author
                                    .WithName(username + " (" + platform.ToUpper() + ")");
                                })
                            .WithColor(new Color(5025616))
                            .WithDescription("Updated: " + updated)
                            .AddInlineField("Kills", kills)
                            .AddInlineField("Downs", downs)
                            .AddInlineField("Revives", revives)
                            .AddInlineField("Time Played", played)
                            .AddInlineField("Matches", matches)
                            .AddInlineField("Scenes Survived", scenes)
                            .AddInlineField("Headshots", headshots)
                            .AddInlineField("Explosive Kills", explosiveKills)
                            .AddInlineField("Shots Landed", shotsLanded)
                            .AddInlineField("Score", score)
                            .AddInlineField("Doors Opened", doorsOpened)
                            .AddInlineField("Season Pass?", seasonPass)

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
        }

        [Group("BO3")]
        [Summary("Retrieve information for Call of Duty: Black Ops III players.")]
        public class BO3 : ModuleBase<SocketCommandContext>
        {
            // To Do
        }
    }
}