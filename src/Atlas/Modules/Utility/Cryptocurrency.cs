using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Atlas.Modules.Utility
{
    public class Cryptocurrency : ModuleBase<SocketCommandContext>
    {
        [Group("crypto")]
        [Summary("To Do")]
        public class Crypto : ModuleBase<SocketCommandContext>
        {
            [Command("coins")]
            [Summary("To Do")]
            public async Task CryptoPrices()
            {
                using (Context.Channel.EnterTypingState())
                {
                    using (var client = new HttpClient())
                    {
                        var json = await client.GetStringAsync("https://api.coinmarketcap.com/v1/ticker/?limit=9");
                        dynamic parse = JsonConvert.DeserializeObject(json);

                        string topCoinID = parse[0].id;

                        string coin1Rank = parse[0].rank;
                        string coin1Name = parse[0].name;
                        string coin1Symbol = parse[0].symbol;
                        double coin1USDInit = parse[0].price_usd;
                        double coin1USD = Math.Round(coin1USDInit, 2);
                        string coin124Change = parse[0].percent_change_24h;

                        string coin2Rank = parse[1].rank;
                        string coin2Name = parse[1].name;
                        string coin2Symbol = parse[1].symbol;
                        double coin2USDInit = parse[1].price_usd;
                        double coin2USD = Math.Round(coin2USDInit, 2);
                        string coin224Change = parse[1].percent_change_24h;

                        string coin3Rank = parse[2].rank;
                        string coin3Name = parse[2].name;
                        string coin3Symbol = parse[2].symbol;
                        double coin3USDInit = parse[2].price_usd;
                        double coin3USD = Math.Round(coin3USDInit, 2);
                        string coin324Change = parse[2].percent_change_24h;

                        string coin4Rank = parse[3].rank;
                        string coin4Name = parse[3].name;
                        string coin4Symbol = parse[3].symbol;
                        double coin4USDInit = parse[3].price_usd;
                        double coin4USD = Math.Round(coin4USDInit, 2);
                        string coin424Change = parse[3].percent_change_24h;

                        string coin5Rank = parse[4].rank;
                        string coin5Name = parse[4].name;
                        string coin5Symbol = parse[4].symbol;
                        double coin5USDInit = parse[4].price_usd;
                        double coin5USD = Math.Round(coin5USDInit, 2);
                        string coin524Change = parse[4].percent_change_24h;

                        string coin6Rank = parse[5].rank;
                        string coin6Name = parse[5].name;
                        string coin6Symbol = parse[5].symbol;
                        double coin6USDInit = parse[5].price_usd;
                        double coin6USD = Math.Round(coin6USDInit, 2);
                        string coin624Change = parse[5].percent_change_24h;

                        string coin7Rank = parse[6].rank;
                        string coin7Name = parse[6].name;
                        string coin7Symbol = parse[6].symbol;
                        double coin7USDInit = parse[6].price_usd;
                        double coin7USD = Math.Round(coin7USDInit, 2);
                        string coin724Change = parse[6].percent_change_24h;

                        string coin8Rank = parse[7].rank;
                        string coin8Name = parse[7].name;
                        string coin8Symbol = parse[7].symbol;
                        double coin8USDInit = parse[7].price_usd;
                        double coin8USD = Math.Round(coin8USDInit, 2);
                        string coin824Change = parse[7].percent_change_24h;

                        string coin9Rank = parse[8].rank;
                        string coin9Name = parse[8].name;
                        string coin9Symbol = parse[8].symbol;
                        double coin9USDInit = parse[8].price_usd;
                        double coin9USD = Math.Round(coin9USDInit, 2);
                        string coin924Change = parse[8].percent_change_24h;

                        var builder = new EmbedBuilder()
                            .WithColor(new Color(5025616))
                            .WithAuthor(author =>
                            {
                                author
                                .WithName("Cryptocurrency Prices")
                                .WithIconUrl("https://files.coinmarketcap.com/static/img/coins/32x32/" + topCoinID + ".png")
                                .WithUrl("https://coinmarketcap.com/");
                            })
                            .AddInlineField(coin1Rank + ". " + coin1Name + " (" + coin1Symbol + ")", "USD: $" + coin1USD.ToString() + "\n24h Change: " + coin124Change + "%")
                            .AddInlineField(coin2Rank + ". " + coin2Name + " (" + coin2Symbol + ")", "USD: $" + coin2USD.ToString() + "\n24h Change: " + coin224Change + "%")
                            .AddInlineField(coin3Rank + ". " + coin3Name + " (" + coin3Symbol + ")", "USD: $" + coin3USD.ToString() + "\n24h Change: " + coin324Change + "%")
                            .AddInlineField(coin4Rank + ". " + coin4Name + " (" + coin4Symbol + ")", "USD: $" + coin4USD.ToString() + "\n24h Change: " + coin424Change + "%")
                            .AddInlineField(coin5Rank + ". " + coin5Name + " (" + coin5Symbol + ")", "USD: $" + coin5USD.ToString() + "\n24h Change: " + coin524Change + "%")
                            .AddInlineField(coin6Rank + ". " + coin6Name + " (" + coin6Symbol + ")", "USD: $" + coin6USD.ToString() + "\n24h Change: " + coin624Change + "%")
                            .AddInlineField(coin7Rank + ". " + coin7Name + " (" + coin7Symbol + ")", "USD: $" + coin7USD.ToString() + "\n24h Change: " + coin724Change + "%")
                            .AddInlineField(coin8Rank + ". " + coin8Name + " (" + coin8Symbol + ")", "USD: $" + coin8USD.ToString() + "\n24h Change: " + coin824Change + "%")
                            .AddInlineField(coin9Rank + ". " + coin9Name + " (" + coin9Symbol + ")", "USD: $" + coin9USD.ToString() + "\n24h Change: " + coin924Change + "%")
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
}
