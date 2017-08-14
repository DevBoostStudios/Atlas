using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Modules.Utility
{
    public class Overwatch : ModuleBase<SocketCommandContext>
    {
        [Command("OW")]
        [Summary("Retrieve statistics for the specified Overwatch player.")]
        public async Task OW(string platform, string region, string username)
        {
            // To Do: If checks on platform, replace platforms to correct, replace # to -

            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync("http://ow-api.herokuapp.com/profile/" + platform + "/" + region + "/" + username);
                dynamic parse = JsonConvert.DeserializeObject(json);

                string level = parse.level;
                string portrait = parse.portrait;
                //string qpWon = parse.games[1].quickplay[0].won;
                //string compWon = parse.games[0].competitive[0].won;
                //string compLost = parse.games[0].competitive[0].lost;
                //string compDraw = parse.games[0].competitive[0].draw;
                //string compPlayed = parse.games[0].competitive[0].played;
                //string qpPlaytime = parse.playtime[0].quickplay;
                //string compPlaytime = parse.playtime[0].competitive;
                //string compRank = parse.competitive[0].rank;
                //string compRankImg = parse.competitive[0].rank_img;

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Overwatch - " + username + " (" + platform + ")")
                        .WithIconUrl(portrait);
                    })
                    //.WithThumbnailUrl(compRankImg)
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