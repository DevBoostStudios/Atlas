﻿using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Atlas.Modules.Utility
{
    public class CallofDuty : ModuleBase<SocketCommandContext>
    {
        [Group("IW")]
        [Summary("Retrieve multiplayer information for Call of Duty: Infinite Warfare players.")]
        public class IW : ModuleBase<SocketCommandContext>
        {
            [Command("stats")]
            [Summary("Retrieve multiplayer statistics for Call of Duty: Infinite Warfare players.")]
            public async Task IWStats(string platform, string username)
            {
                // Set Profile URL
                string url = "https://my.callofduty.com/iw/stats?platform=" + platform + "&username=" + username;

                // Fetch Profile HTML
                HtmlDocument htmlDoc = new HtmlWeb().Load(url);

                // Get Prestige Value
                HtmlNode Body = htmlDoc.DocumentNode.SelectNodes("//body[@class='with-sso-bar desktop sso-auth-known']")[0];
                HtmlNode PageContentContainer = Body.SelectNodes("//div[@class='page-content-container']")[0];
                HtmlNode PageContentParsys = PageContentContainer.SelectNodes("//div[@class='page-content parsys']")[0];
                HtmlNode AtviComponentAtvi = PageContentParsys.SelectNodes("//div[@class='atvi-component atvi-content-tile ignore-id template  ']")[0];
                HtmlNode MyCod = AtviComponentAtvi.SelectNodes("//div[@id='mycod']")[0];
                HtmlNode App = MyCod.SelectNodes("//div[@id='app']")[0];
                HtmlNode AppHeader = App.SelectNodes("//div[@class='app-header']")[0];
                HtmlNode AppControls = AppHeader.SelectNodes("//div[@class='app-controls']")[0];
                HtmlNode Account = AppControls.SelectNodes("//section[@class='account']")[0];
                HtmlNode AccountInner = Account.SelectNodes("//div[@class='account-inner inner-wrapper']")[0];
                HtmlNode AccountText = AccountInner.SelectNodes("//div[@class='account-text']")[0];
                HtmlNode Level = AccountText.SelectNodes("//div[@class='level']")[0];
                HtmlNode LevelInner = Level.SelectNodes("//div[@class='level-inner']")[0];
                var PrestigeValue = LevelInner.SelectNodes("//span[@class='prestige value']")[0].InnerText;

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