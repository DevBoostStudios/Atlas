using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Atlas.Modules.Utility
{
    public class Overwatch : ModuleBase<SocketCommandContext>
    {
        [Command("OW")]
        [Summary("Retrieve statistics for the specified Overwatch player.")]
        public async Task OW(string platform, string region, string username)
        {
            // To Do: Replace invalid platform and region strings with correct, replace # in username with -
            //if (platform == "PC")
            //{
            //    username.Replace('#', '-');
            //}

            //if (platform == "PS4")
            //{
            //    platform.Replace("PS4", "PSN");
            //}

            using (var client = new HttpClient())
            {
                var profileJSON = await client.GetStringAsync("http://ow-api.herokuapp.com/profile/" + platform + "/" + region + "/" + username);
                dynamic profileParse = JsonConvert.DeserializeObject(profileJSON);

                var statsJSON = await client.GetStringAsync("http://ow-api.herokuapp.com/stats/" + platform + "/" + region + "/" + username);
                dynamic statsParse = JsonConvert.DeserializeObject(statsJSON);

                string portrait = profileParse.portrait;
                string level = profileParse.level;
                string sr = profileParse.competitive.rank;

                string qpPlaytime = profileParse.playtime.quickplay;
                string qpElims = statsParse.stats.combat.quickplay[4].value;
                string qpDeaths = statsParse.stats.deaths.quickplay[0].value;
                string qpWins = profileParse.games.quickplay.won;
                string qpLosses = "0";
                string qpDraws = "0";
                string qpDamage = statsParse.stats.combat.quickplay[5].value;
                string qpStreak = statsParse.stats.miscellaneous.quickplay[5].value;
                string qpMostPlayed = statsParse.stats.top_heroes.quickplay[0].hero;

                string compPlaytime = profileParse.playtime.competitive;
                string compElims = statsParse.stats.combat.competitive[4].value;
                string compDeaths = statsParse.stats.deaths.competitive[0].value;
                string compWins = profileParse.games.competitive.won;
                string compLosses = profileParse.games.competitive.lost;
                string compDraws = profileParse.games.competitive.draw;
                string compDamage = statsParse.stats.combat.competitive[5].value;
                string compStreak = statsParse.stats.miscellaneous.competitive[5].value;
                string compMostPlayed = statsParse.stats.top_heroes.competitive[0].hero;

                string qpTime = Regex.Match(qpPlaytime, @"\d+").Value;
                string compTime = Regex.Match(compPlaytime, @"\d+").Value;
                string playtime = Int32.Parse(qpTime) + Int32.Parse(compTime) + " hours";

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName(username + " (" + platform + ")")
                        .WithIconUrl(portrait);
                    })
                    .AddInlineField("Level", level)
                    .AddInlineField("SR", sr)
                    .AddInlineField("Playtime", playtime)
                    .AddField("​", "__**Quickplay**__") // Zero-Width Space
                    .AddInlineField("Playtime", qpPlaytime)
                    .AddInlineField("Eliminations", qpElims)
                    .AddInlineField("Deaths", qpDeaths)
                    .AddInlineField("Wins", qpWins)
                    .AddInlineField("Losses", qpLosses)
                    .AddInlineField("Draws", qpDraws)
                    .AddInlineField("Damage", qpDamage)
                    .AddInlineField("Streak", qpStreak)
                    .AddInlineField("Most Played", qpMostPlayed)
                    .AddField("​", "__**Current Competitive Season**__") // Zero-Width Space - // To Do: Cumulative Competitive Stats or a way to pull current Season #
                    .AddInlineField("Playtime", compPlaytime)
                    .AddInlineField("Eliminations", compElims)
                    .AddInlineField("Deaths", compDeaths)
                    .AddInlineField("Wins", compWins)
                    .AddInlineField("Losses", compLosses)
                    .AddInlineField("Draws", compDraws)
                    .AddInlineField("Damage", compDamage)
                    .AddInlineField("Streak", compStreak)
                    .AddInlineField("Most Played", compMostPlayed)
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

// Profile JSON
//{
//	"username": "ImMotive__",
//	"level": 241,
//	"portrait": "https://blzgdapipro-a.akamaihd.net/game/unlocks/0x02500000000009E6.png",
//	"games": {
//		"quickplay": {
//			"won": 174
//		},
//		"competitive": {
//			"won": 133,
//			"lost": 133,
//			"draw": 7,
//			"played": 273
//		}
//	},
//	"playtime": {
//		"quickplay": "41 hours",
//		"competitive": "57 hours"
//	},
//	"competitive": {
//		"rank": 2775,
//		"rank_img": "https://blzgdapipro-a.akamaihd.net/game/rank-icons/season-2/rank-4.png"
//	},
//	"levelFrame": "https://blzgdapipro-a.akamaihd.net/game/playerlevelrewards/0x025000000000093B_Border.png",
//	"star": "https://blzgdapipro-a.akamaihd.net/game/playerlevelrewards/0x025000000000093B_Rank.png"
//}

// Stats JSON
//{
//	"username": "ImMotive__",
//	"level": 241,
//	"portrait": "https://blzgdapipro-a.akamaihd.net/game/unlocks/0x02500000000009E6.png",
//	"stats": {
//		"top_heroes": {
//			"quickplay": [{
//				"hero": "Bastion",
//				"played": "17 hours",
//				"img": "https://blzgdapipro-a.akamaihd.net/game/heroes/small/0x02E0000000000015.png"
//			},
         // More here so do [0] or w/e
//			"competitive": [{
//				"hero": "Soldier: 76",
//				"played": "13 hours",
//				"img": "https://blzgdapipro-a.akamaihd.net/game/heroes/small/0x02E000000000006E.png"
//			},
        // More here so do [0] or w/e
//		},
//		"combat": {
//			"quickplay": [{
//				"title": "Melee Final Blows",
//				"value": "15"
//			}, {
//				"title": "Solo Kills",
//				"value": "970"
//			}, {
//				"title": "Objective Kills",
//				"value": "2,212"
//			}, {
//				"title": "Final Blows",
//				"value": "2,884"
//			}, {
//				"title": "Eliminations",
//				"value": "4,747"
//			}, {
//				"title": "All Damage Done",
//				"value": "2,007,027"
//			}, {
//				"title": "Environmental Kills",
//				"value": "7"
//			}, {
//				"title": "Multikills",
//				"value": "35"
//			}],
//			"competitive": [{
//				"title": "Melee Final Blows",
//				"value": "12"
//			}, {
//				"title": "Solo Kills",
//				"value": "788"
//			}, {
//				"title": "Objective Kills",
//				"value": "3,844"
//			}, {
//				"title": "Final Blows",
//				"value": "3,841"
//			}, {
//				"title": "Eliminations",
//				"value": "6,990"
//			}, {
//				"title": "All Damage Done",
//				"value": "3,662,396"
//			}, {
//				"title": "Environmental Kills",
//				"value": "65"
//			}, {
//				"title": "Multikills",
//				"value": "77"
//			}]
//		},
//		"deaths": {
//			"quickplay": [{
//				"title": "Deaths",
//				"value": "2,070"
//			}],
//			"competitive": [{
//				"title": "Deaths",
//				"value": "2,981"
//			}]
//		},
//		"match_awards": {
//			"quickplay": [{
//				"title": "Cards",
//				"value": "145"
//			}, {
//				"title": "Medals",
//				"value": "1,095"
//			}, {
//				"title": "Medals - Gold",
//				"value": "476"
//			}, {
//				"title": "Medals - Silver",
//				"value": "361"
//			}, {
//				"title": "Medals - Bronze",
//				"value": "258"
//			}],
//			"competitive": [{
//				"title": "Cards",
//				"value": "111"
//			}, {
//				"title": "Medals",
//				"value": "1,008"
//			}, {
//				"title": "Medals - Gold",
//				"value": "436"
//			}, {
//				"title": "Medals - Silver",
//				"value": "307"
//			}, {
//				"title": "Medals - Bronze",
//				"value": "264"
//			}]
//		},
//		"assists": {
//			"quickplay": [{
//				"title": "Healing Done",
//				"value": "265,244"
//			}, {
//				"title": "Recon Assists",
//				"value": "11"
//			}, {
//				"title": "Teleporter Pads Destroyed",
//				"value": "3"
//			}],
//			"competitive": [{
//				"title": "Healing Done",
//				"value": "430,415"
//			}, {
//				"title": "Recon Assists",
//				"value": "3"
//			}, {
//				"title": "Teleporter Pads Destroyed",
//				"value": "14"
//			}]
//		},
//		"average": {
//			"quickplay": [{
//				"title": "All Damage Done - Avg per 10 Min",
//				"value": "14"
//			}],
//			"competitive": [{
//				"title": "All Damage Done - Avg per 10 Min",
//				"value": "18"
//			}]
//		},
//		"miscellaneous": {
//			"quickplay": [{
//				"title": "Melee Final Blows - Most in Game",
//				"value": "2"
//			}, {
//				"title": "Shield Generator Destroyed - Most in Game",
//				"value": "1"
//			}, {
//				"title": "Turrets Destroyed - Most in Game",
//				"value": "5"
//			}, {
//				"title": "Environmental Kills - Most in Game",
//				"value": "3"
//			}, {
//				"title": "Teleporter Pad Destroyed - Most in Game",
//				"value": "1"
//			}, {
//				"title": "Kill Streak - Best",
//				"value": "36"
//			}, {
//				"title": "Hero Damage Done - Most in Game",
//				"value": "5,259"
//			}, {
//				"title": "Barrier Damage Done",
//				"value": "8,670"
//			}, {
//				"title": "Barrier Damage Done - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Deaths - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Hero Damage Done",
//				"value": "76,737"
//			}, {
//				"title": "Hero Damage Done - Avg per 10 Min",
//				"value": "6"
//			}, {
//				"title": "Time Spent on Fire - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Solo Kills - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Objective Time - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Objective Kills - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Healing Done - Avg per 10 Min",
//				"value": "2"
//			}, {
//				"title": "Final Blows - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Eliminations - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Shield Generator Destroyed",
//				"value": "1"
//			}, {
//				"title": "Turrets Destroyed",
//				"value": "234"
//			}, {
//				"title": "Recon Assists - Most in Game",
//				"value": "8"
//			}, {
//				"title": "Offensive Assists",
//				"value": "22"
//			}, {
//				"title": "Defensive Assists",
//				"value": "11"
//			}],
//			"competitive": [{
//				"title": "Melee Final Blow - Most in Game",
//				"value": "1"
//			}, {
//				"title": "Shield Generator Destroyed - Most in Game",
//				"value": "1"
//			}, {
//				"title": "Turrets Destroyed - Most in Game",
//				"value": "16"
//			}, {
//				"title": "Environmental Kills - Most in Game",
//				"value": "6"
//			}, {
//				"title": "Teleporter Pad Destroyed - Most in Game",
//				"value": "1"
//			}, {
//				"title": "Kill Streak - Best",
//				"value": "67"
//			}, {
//				"title": "Hero Damage Done - Most in Game",
//				"value": "22,874"
//			}, {
//				"title": "Barrier Damage Done",
//				"value": "726,635"
//			}, {
//				"title": "Barrier Damage Done - Avg per 10 Min",
//				"value": "3"
//			}, {
//				"title": "Deaths - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Hero Damage Done",
//				"value": "1,921,037"
//			}, {
//				"title": "Hero Damage Done - Avg per 10 Min",
//				"value": "12"
//			}, {
//				"title": "Time Spent on Fire - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Solo Kills - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Objective Time - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Objective Kills - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Healing Done - Avg per 10 Min",
//				"value": "2"
//			}, {
//				"title": "Final Blows - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Eliminations - Avg per 10 Min",
//				"value": "0"
//			}, {
//				"title": "Shield Generators Destroyed",
//				"value": "7"
//			}, {
//				"title": "Turrets Destroyed",
//				"value": "617"
//			}, {
//				"title": "Games Tied",
//				"value": "7"
//			}, {
//				"title": "Games Lost",
//				"value": "133"
//			}, {
//				"title": "Recon Assists - Most in Game",
//				"value": "3"
//			}, {
//				"title": "Offensive Assists",
//				"value": "221"
//			}, {
//				"title": "Defensive Assists",
//				"value": "323"
//			}]
//		},
//		"best": {
//			"quickplay": [{
//				"title": "Eliminations - Most in Game",
//				"value": "45"
//			}, {
//				"title": "Final Blows - Most in Game",
//				"value": "30"
//			}, {
//				"title": "All Damage Done - Most in Game",
//				"value": "25,076"
//			}, {
//				"title": "Healing Done - Most in Game",
//				"value": "4,787"
//			}, {
//				"title": "Defensive Assists - Most in Game",
//				"value": "10"
//			}, {
//				"title": "Offensive Assists - Most in Game",
//				"value": "9"
//			}, {
//				"title": "Objective Kills - Most in Game",
//				"value": "28"
//			}, {
//				"title": "Objective Time - Most in Game",
//				"value": "03:27"
//			}, {
//				"title": "Multikill - Best",
//				"value": "4"
//			}, {
//				"title": "Solo Kills - Most in Game",
//				"value": "30"
//			}, {
//				"title": "Time Spent on Fire - Most in Game",
//				"value": "09:39"
//			}],
//			"competitive": [{
//				"title": "Eliminations - Most in Game",
//				"value": "67"
//			}, {
//				"title": "Final Blows - Most in Game",
//				"value": "41"
//			}, {
//				"title": "All Damage Done - Most in Game",
//				"value": "37,566"
//			}, {
//				"title": "Healing Done - Most in Game",
//				"value": "14,971"
//			}, {
//				"title": "Defensive Assists - Most in Game",
//				"value": "29"
//			}, {
//				"title": "Offensive Assists - Most in Game",
//				"value": "13"
//			}, {
//				"title": "Objective Kills - Most in Game",
//				"value": "59"
//			}, {
//				"title": "Objective Time - Most in Game",
//				"value": "07:01"
//			}, {
//				"title": "Multikill - Best",
//				"value": "5"
//			}, {
//				"title": "Solo Kills - Most in Game",
//				"value": "41"
//			}, {
//				"title": "Time Spent on Fire - Most in Game",
//				"value": "11:37"
//			}]
//		},
//		"game": {
//			"quickplay": [{
//				"title": "Time Played",
//				"value": "41 hours"
//			}, {
//				"title": "Time Spent on Fire",
//				"value": "05:17:36"
//			}, {
//				"title": "Objective Time",
//				"value": "03:41:05"
//			}, {
//				"title": "Games Won",
//				"value": "174"
//			}],
//			"competitive": [{
//				"title": "Time Played",
//				"value": "57 hours"
//			}, {
//				"title": "Time Spent on Fire",
//				"value": "08:12:59"
//			}, {
//				"title": "Objective Time",
//				"value": "08:03:40"
//			}, {
//				"title": "Games Played",
//				"value": "273"
//			}, {
//				"title": "Games Won",
//				"value": "133"
//			}]
//		}
//	}
//}