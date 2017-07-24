using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Discord.WebSocket;
using System.Linq;

namespace Maya.Modules.Utility
{
    public class CallofDuty : ModuleBase<SocketCommandContext>
    {
        [Group("IW")]
        [Summary("Retrieve multiplayer information for Call of Duty: Infinite Warfare players.")]
        public class IW : ModuleBase<SocketCommandContext>
        {
            [Command("stats")]
            [Summary("Retrieve multiplayer statistics for Call of Duty: Infinite Warfare players.")]
            public async Task stats(string platform, string username)
            {
                // Set Profile URL
                string url = "https://my.callofduty.com/iw/stats?platform=" + platform + "&username=" + username;

                // Fetch Profile HTML
                HtmlDocument htmlDoc = new HtmlWeb().Load(url);

                // Parse HTML Values
                string PrestigeValue = htmlDoc.DocumentNode.SelectNodes("//span[@class='prestige value']")[0].InnerText;

                var builder = new EmbedBuilder()
                    .WithAuthor(author =>
                    {
                        author
                        .WithName(username + " (" + platform + ")")
                        .WithIconUrl("https://my.callofduty.com/content/dam/atvi/callofduty/mycod/common/player-icons/iw/prestige-" + PrestigeValue + ".png");
                    })
                    .WithColor(new Color(5025616))
                    .AddInlineField("Prestige", PrestigeValue)
                    .AddInlineField("Rank", "55")
                    .AddInlineField("Played", "12 Days")
                    .AddInlineField("Kills", "123,456")
                    .AddInlineField("Deaths", "789,123")
                    .AddInlineField("Ratio", "1.23")
                    .AddInlineField("Wins", "123")
                    .AddInlineField("Losses", "456")
                    .AddInlineField("Ratio", "1.23")
                    .AddInlineField("Score", "1,234,567")
                    .AddInlineField("SPM", "123")
                    .AddInlineField("Matches", "1234")
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