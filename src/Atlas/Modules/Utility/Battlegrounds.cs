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
    public class Battlegrounds : ModuleBase<SocketCommandContext>
    {
        private IConfiguration _config;

        [Command("pubg", RunMode = RunMode.Async)]
        [Summary("Retrieves detailed statistics for the specified Player Unknown's Battlegrounds player.")]
        public async Task PUBG([Remainder] string username)
        {
            using (Context.Channel.EnterTypingState())
            {
                using (var client = new HttpClient())
                {
                    _config = BuildConfig();

                    client.DefaultRequestHeaders.Add("TRN-Api-Key", _config["trn-key"]);
                    var json = await client.GetStringAsync("https://api.pubgtracker.com/v2/profile/pc/" + username);
                    dynamic parse = JsonConvert.DeserializeObject(json);

                    string nickname = parse.nickname;
                    string avatar = parse.avatar;
                    string lastUpdated = parse.lastUpdated;

                    string duoKD = parse.stats[0].stats[0].displayValue;
                    string duoKDPerct = parse.stats[0].stats[0].percentile;
                    string duoWL = parse.stats[0].stats[1].displayValue;
                    string duoWLPerct = parse.stats[0].stats[1].percentile;

                    var builder = new EmbedBuilder()
                        .WithColor(new Color(5025616))
                        .WithAuthor(author =>
                        {
                            author
                            .WithName("Player Unknown's Battlegrounds - " + nickname)
                            .WithIconUrl(avatar);
                        })
                        .WithUrl("https://pubgtracker.com/profile/pc/" + username)
                        .WithDescription("Updated: " + lastUpdated)
                        .AddField("​", "__**Duo**__")
                        .AddInlineField("K/D Ratio", duoKD + " (Top " + duoKDPerct + "%)")
                        .AddInlineField("Win Percentage", duoWL + " (Top " + duoWLPerct + "%)")
                        .AddField("​", "__**Squad**__")
                        .AddInlineField("Test", "test")
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
