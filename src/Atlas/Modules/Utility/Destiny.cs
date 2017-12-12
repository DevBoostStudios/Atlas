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
    public class Destiny : ModuleBase<SocketCommandContext>
    {
        [Group("Destiny2")]
        [Summary("Retrieve information for Destiny 2.")]
        public class Destiny2 : ModuleBase<SocketCommandContext>
        {
            [Group("stats")]
            [Summary("Retrieve statistics for the specified Destiny 2 player.")]
            public class Destiny2Stats : ModuleBase<SocketCommandContext>
            {
                private IConfiguration _config;

                [Command("pvp", RunMode = RunMode.Async)]
                [Summary("Retrieve PvP Statistics for the specified Destiny 2 player.")]
                public async Task D2Stats([Remainder] string username)
                {
                    using (Context.Channel.EnterTypingState())
                    {
                        using (var client = new HttpClient())
                        {
                            _config = BuildConfig();

                            client.DefaultRequestHeaders.Add("X-API-Key", _config["bungieKey"]);
                            var json = await client.GetStringAsync("https://www.bungie.net/Platform/Destiny2/SearchDestinyPlayer/all/" + username + "/");
                            dynamic parse = JsonConvert.DeserializeObject(json);

                            string membershipType = parse.Response[0].membershipType;
                            string membershipId = parse.Response[0].membershipId;
                            string displayName = parse.Response[0].displayName;
                            string platform = (membershipType == "1") ? "Xbox" : (membershipType == "2") ? "PSN" : (membershipType == "4") ? "PC" : "Unknown";

                            var profileJSON = await client.GetStringAsync("https://www.bungie.net/Platform/Destiny2/" + membershipType + "/Profile/" + membershipId + "/?components=100");
                            dynamic profileParse = JsonConvert.DeserializeObject(profileJSON);

                            string character1ID = profileParse.Response.profile.data.characterIds[0];
                            string character2ID = profileParse.Response.profile.data.characterIds[1];
                            string character3ID = profileParse.Response.profile.data.characterIds[2];

                            var ch1JSON = await client.GetStringAsync("https://www.bungie.net/Platform/Destiny2/" + membershipType + "/Profile/" + membershipId + "/Character/" + character1ID + "/?components=200");
                            dynamic ch1Parse = JsonConvert.DeserializeObject(ch1JSON);

                            string ch1Emblem = "http://bungie.net" + ch1Parse.Response.character.data.emblemPath;
                            string ch1ClassInit = ch1Parse.Response.character.data.classType;
                            string ch1Class = (ch1ClassInit == "0") ? "Titan" : (ch1ClassInit == "1") ? "Hunter" : (ch1ClassInit == "2") ? "Warlock" : "Unknown";
                            string ch1GenderInit = ch1Parse.Response.character.data.genderType;
                            string ch1Gender = (ch1GenderInit == "0") ? "Male" : (ch1GenderInit == "1") ? "Female" : "Unknown";
                            string ch1RaceInit = ch1Parse.Response.character.data.raceType;
                            string ch1Race = (ch1RaceInit == "0") ? "Human" : (ch1RaceInit == "1") ? "Awoken" : (ch1RaceInit == "2") ? "Exo" : "Unknown";
                            string ch1Level = ch1Parse.Response.character.data.levelProgression.level;
                            string ch1Light = ch1Parse.Response.character.data.light;

                            var ch1StatsJSON = await client.GetStringAsync("https://www.bungie.net/Platform/Destiny2/" + membershipType + "/Account/" + membershipId + "/Character/" + character1ID + "/Stats/");
                            dynamic ch1StatsParse = JsonConvert.DeserializeObject(ch1StatsJSON);

                            int ch1PvPSecondsPlayed = ch1StatsParse.Response.allPvP.allTime.secondsPlayed.basic.value;
                            TimeSpan ch1PvPPlayTime = TimeSpan.FromSeconds(ch1PvPSecondsPlayed);
                            string ch1PvPTimePlayed = string.Format("{0:D2} Days {1:D2} Hours",
                                ch1PvPPlayTime.Days,
                                ch1PvPPlayTime.Hours);
                            string ch1PvPKD = ch1StatsParse.Response.allPvP.allTime.killsDeathsRatio.basic.displayValue;
                            string ch1PvPWL = ch1StatsParse.Response.allPvP.allTime.winLossRatio.basic.displayValue;
                            string ch1PvPAssists = ch1StatsParse.Response.allPvP.allTime.assists.basic.displayValue;

                            var ch2JSON = await client.GetStringAsync("https://www.bungie.net/Platform/Destiny2/" + membershipType + "/Profile/" + membershipId + "/Character/" + character2ID + "/?components=200");
                            dynamic ch2Parse = JsonConvert.DeserializeObject(ch2JSON);

                            string ch2ClassInit = ch2Parse.Response.character.data.classType;
                            string ch2Class = (ch2ClassInit == "0") ? "Titan" : (ch2ClassInit == "1") ? "Hunter" : (ch2ClassInit == "2") ? "Warlock" : "Unknown";
                            string ch2GenderInit = ch2Parse.Response.character.data.genderType;
                            string ch2Gender = (ch2GenderInit == "0") ? "Male" : (ch2GenderInit == "1") ? "Female" : "Unknown";
                            string ch2RaceInit = ch2Parse.Response.character.data.raceType;
                            string ch2Race = (ch2RaceInit == "0") ? "Human" : (ch2RaceInit == "1") ? "Awoken" : (ch2RaceInit == "2") ? "Exo" : "Unknown";
                            string ch2Level = ch2Parse.Response.character.data.levelProgression.level;
                            string ch2Light = ch2Parse.Response.character.data.light;

                            var ch2StatsJSON = await client.GetStringAsync("https://www.bungie.net/Platform/Destiny2/" + membershipType + "/Account/" + membershipId + "/Character/" + character2ID + "/Stats/");
                            dynamic ch2StatsParse = JsonConvert.DeserializeObject(ch2StatsJSON);

                            int ch2PvPSecondsPlayed = ch2StatsParse.Response.allPvP.allTime.secondsPlayed.basic.value;
                            TimeSpan ch2PvPPlayTime = TimeSpan.FromSeconds(ch2PvPSecondsPlayed);
                            string ch2PvPTimePlayed = string.Format("{0:D2} Days {1:D2} Hours",
                                ch2PvPPlayTime.Days,
                                ch2PvPPlayTime.Hours);
                            string ch2PvPKD = ch2StatsParse.Response.allPvP.allTime.killsDeathsRatio.basic.displayValue;
                            string ch2PvPWL = ch2StatsParse.Response.allPvP.allTime.winLossRatio.basic.displayValue;
                            string ch2PvPAssists = ch2StatsParse.Response.allPvP.allTime.assists.basic.displayValue;

                            var ch3JSON = await client.GetStringAsync("https://www.bungie.net/Platform/Destiny2/" + membershipType + "/Profile/" + membershipId + "/Character/" + character3ID + "/?components=200");
                            dynamic ch3Parse = JsonConvert.DeserializeObject(ch3JSON);

                            string ch3ClassInit = ch3Parse.Response.character.data.classType;
                            string ch3Class = (ch3ClassInit == "0") ? "Titan" : (ch3ClassInit == "1") ? "Hunter" : (ch3ClassInit == "2") ? "Warlock" : "Unknown";
                            string ch3GenderInit = ch3Parse.Response.character.data.genderType;
                            string ch3Gender = (ch3GenderInit == "0") ? "Male" : (ch3GenderInit == "1") ? "Female" : "Unknown";
                            string ch3RaceInit = ch3Parse.Response.character.data.raceType;
                            string ch3Race = (ch3RaceInit == "0") ? "Human" : (ch3RaceInit == "1") ? "Awoken" : (ch3RaceInit == "2") ? "Exo" : "Unknown";
                            string ch3Level = ch3Parse.Response.character.data.levelProgression.level;
                            string ch3Light = ch3Parse.Response.character.data.light;

                            var ch3StatsJSON = await client.GetStringAsync("https://www.bungie.net/Platform/Destiny2/" + membershipType + "/Account/" + membershipId + "/Character/" + character3ID + "/Stats/");
                            dynamic ch3StatsParse = JsonConvert.DeserializeObject(ch3StatsJSON);

                            int ch3PvPSecondsPlayed = ch3StatsParse.Response.allPvP.allTime.secondsPlayed.basic.value;
                            TimeSpan ch3PvPPlayTime = TimeSpan.FromSeconds(ch3PvPSecondsPlayed);
                            string ch3PvPTimePlayed = string.Format("{0:D2} Days {1:D2} Hours",
                                ch3PvPPlayTime.Days,
                                ch3PvPPlayTime.Hours);
                            string ch3PvPKD = ch3StatsParse.Response.allPvP.allTime.killsDeathsRatio.basic.displayValue;
                            string ch3PvPWL = ch3StatsParse.Response.allPvP.allTime.winLossRatio.basic.displayValue;
                            string ch3PvPAssists = ch3StatsParse.Response.allPvP.allTime.assists.basic.displayValue;

                            var builder = new EmbedBuilder()
                                .WithColor(new Color(5025616))
                                .WithAuthor(author =>
                                {
                                    author
                                    .WithName("Destiny 2 - " + displayName + " (" + platform.ToUpper() + ")")
                                    .WithIconUrl(ch1Emblem)
                                    .WithUrl("https://destinytracker.com/d2/profile/" + platform + "/" + displayName);
                                })
                                .AddField("​", "__**" + ch1Class + " (" + ch1Gender + " " + ch1Race + ")**__")
                                .AddInlineField("Level", ch1Level)
                                .AddInlineField("Light", ch1Light)
                                .AddInlineField("PvP Time Played", ch1PvPTimePlayed)
                                .AddInlineField("PvP K/D", ch1PvPKD)
                                .AddInlineField("PvP W/L", ch1PvPWL)
                                .AddInlineField("PvP Assists", ch1PvPAssists)
                                .AddField("​", "__**" + ch2Class + " (" + ch2Gender + " " + ch2Race + ")**__")
                                .AddInlineField("Level", ch2Level)
                                .AddInlineField("Light", ch2Light)
                                .AddInlineField("PvP Time Played", ch2PvPTimePlayed)
                                .AddInlineField("PvP K/D", ch2PvPKD)
                                .AddInlineField("PvP W/L", ch2PvPWL)
                                .AddInlineField("PvP Assists", ch2PvPAssists)
                                .AddField("​", "__**" + ch3Class + " (" + ch3Gender + " " + ch3Race + ")**__")
                                .AddInlineField("Level", ch3Level)
                                .AddInlineField("Light", ch3Light)
                                .AddInlineField("PvP Time Played", ch3PvPTimePlayed)
                                .AddInlineField("PvP K/D", ch3PvPKD)
                                .AddInlineField("PvP W/L", ch3PvPWL)
                                .AddInlineField("PvP Assists", ch3PvPAssists)
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
    }
}
