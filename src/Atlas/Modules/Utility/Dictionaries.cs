using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Atlas.Modules.Utility
{
    public class Dictionaries : ModuleBase<SocketCommandContext>
    {
        [Command("urban")]
        [Summary("Returns the top result for the specified word from Urban Dictionary.")]
        public async Task Urban([Remainder] string word)
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync("http://api.urbandictionary.com/v0/define?term=" + word);
                dynamic parse = JsonConvert.DeserializeObject(json);

                string definition = parse.list[0].definition;
                string example = parse.list[0].example;
                string urbanLink = parse.list[0].permalink;
                string upvotes = parse.list[0].thumbs_up;
                string downvotes = parse.list[0].thumbs_down;

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Urban Dictionary - \"" + word.Substring(0, 1).ToUpper() + word.Substring(1) + "\"")
                        .WithIconUrl("http://i.imgur.com/bvxX7Fm.jpg");
                    })
                    .WithUrl(urbanLink)
                    .WithDescription(definition)
                    .AddInlineField("Thumbs Up", upvotes)
                    .AddInlineField("Thumbs Down", downvotes)
                    .AddField("Example", example)
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

        [Command("hashtag")]
        [Summary("Returns the top result for the specified hashtag from TagDef.")]
        public async Task Hashtag(string hashtag)
        {
            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync("https://api.tagdef.com/one." + hashtag + ".json");
                dynamic parse = JsonConvert.DeserializeObject(json);

                string definition = parse.defs.def.text;
                string created = parse.defs.def.time;
                string upvotes = parse.defs.def.upvotes;
                string downvotes = parse.defs.def.downvotes;

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("TagDef - #" + hashtag.Substring(0, 1).ToUpper() + hashtag.Substring(1))
                        .WithIconUrl("http://i.imgur.com/odf04Fm.jpg");
                    })
                    .WithUrl("https://tagdef.com/en/tag/" + hashtag)
                    .WithDescription(definition)
                    .AddInlineField("Thumbs Up", upvotes)
                    .AddInlineField("Thumbs Down", downvotes)
                    .AddInlineField("Created", created)
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
