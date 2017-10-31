using Discord.Commands;
using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Atlas.Modules.Utility
{
    public class Fortnite : ModuleBase<SocketCommandContext>
    {
        [Command("fortnite")]
        [Summary("To Do")]
        public async Task FortniteStats(string platform, [Remainder] string username)
        {
            using (Context.Channel.EnterTypingState())
            {
                using (var client = new HttpClient())
                {
                    var htmlDoc = await client.GetStringAsync("https://fortnitetracker.com/profile/" + platform + "/" + username); // To Do: Sanitize Platforms and Usernames if necessary (PS4 -> PSN, # -> -, etc)

                    Match match = Regex.Match(htmlDoc, "<script type=\"text / javascript\">", RegexOptions.IgnoreCase);
                    string playerStats = match.Groups[1].Value;


                    // Debug
                    Console.WriteLine(playerStats);
                    await ReplyAsync("Debug: Reached end of `FortniteStats()`, see console. Should've printed `playerStats`.");
                }
            }
        }
    }
}
