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
    public class Translator : ModuleBase<SocketCommandContext>
    {
        [Group("translate")]
        [Summary("Translation commands.")]
        public class Translation : ModuleBase<SocketCommandContext>
        {
            private IConfiguration _config;

            private IConfiguration BuildConfig()
            {
                return new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json")
                    .Build();
            }

            [Command]
            [Summary("Translate the specified input to a specified language.")]
            public async Task Translate(string outputLang, [Remainder] string input)
            {
                using (Context.Channel.EnterTypingState())
                {
                    using (var client = new HttpClient())
                    {
                        _config = BuildConfig();

                        var json = await client.GetStringAsync("https://translate.yandex.net/api/v1.5/tr.json/translate?text=" + input + "&detect?hint=en&lang=" + outputLang + "&key=" + _config["yandexKey"]);
                        dynamic parse = JsonConvert.DeserializeObject(json);

                        string output = parse.text[0];
                        string inputLangInit = parse.lang;
                        string inputLang = inputLangInit.Split('-')[0];

                        var builder = new EmbedBuilder()
                            .WithColor(new Color(5025616))
                            .WithAuthor(author =>
                            {
                                author
                                .WithName("Translate")
                                .WithIconUrl("http://i.imgur.com/qHGDulG.png");
                            })
                            .AddField("Input (" + inputLang + ")", input)
                            .AddField("Output (" + outputLang + ")", output)
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

            [Command("languages")]
            [Summary("Returns a list of languages available for translation.")]
            public async Task TransLangs()
            {
                using (Context.Channel.EnterTypingState())
                {
                    using (var client = new HttpClient())
                    {
                        _config = BuildConfig();
                        var yandexKey = _config["yandexKey"];

                        var json = await client.GetStringAsync("https://translate.yandex.net/api/v1.5/tr.json/getLangs?ui=en&key=" + yandexKey);
                        dynamic parse = JsonConvert.DeserializeObject(json);

                        var builder = new EmbedBuilder()
                            .WithColor(new Color(5025616))
                            .WithAuthor(author =>
                            {
                                author
                                .WithName("Translate")
                                .WithIconUrl("http://i.imgur.com/qHGDulG.png");
                            })
                            .AddInlineField("Language", "To Do") // To Do: Take langs from parse and make an InlineField for each
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
