using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Atlas.Modules.Utility
{
    public class Ciphers : ModuleBase<SocketCommandContext>
    {
        [Group("morse")]
        [Summary("To Do")]
        public class Morse : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task MorseEncode([Remainder] string text)
            {
                await ReplyAsync("Debug");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task MorseDecode([Remainder] string cipher)
            {
                await ReplyAsync("Debug");
            }
        }

        [Group("A1Z26")]
        [Summary("To Do")]
        public class A1Z26 : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task A1Z26Encode([Remainder] string text)
            {
                await ReplyAsync("Debug");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task A1Z26Decode([Remainder] string cipher)
            {
                await ReplyAsync("Debug");
            }
        }

        [Group("caesar")]
        [Summary("To Do")]
        public class Caesar : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task CaesarEncode(int shift, [Remainder] string text)
            {
                await ReplyAsync("Debug");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task CaesarDecode(int shift, [Remainder] string cipher)
            {
                await ReplyAsync("Debug");
            }
        }

        [Group("base64")]
        [Summary("Base64, also known as MIME encoding, translates binary into safe text. It is used to send attachments in email and to change small bits of unsafe high-character data into stuff that is a lot nicer for text-based system.")]
        public class Base64 : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("Encodes the specified Text in Base64.")]
            public async Task Base64Encode([Remainder] string text)
            {
                var textBytes = System.Text.Encoding.UTF8.GetBytes(text);
                var encodedText = Convert.ToBase64String(textBytes);

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Base64")
                        .WithIconUrl("http://i.imgur.com/ONH5AnP.png");
                    })
                    .AddField("Plaintext", "`" + text + "`")
                    .AddField("Cipher Text", "`" + encodedText + "`")
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

            [Command("decode")]
            [Summary("Decodes the specified Base64 Ciphertext to Plaintext.")]
            public async Task Base64Decode([Remainder] string cipher)
            {
                var base64EncodedBytes = Convert.FromBase64String(cipher);
                var decodedText = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Base64")
                        .WithIconUrl("http://i.imgur.com/ONH5AnP.png");
                    })
                    .AddField("Cipher Text", "`" + cipher + "`")
                    .AddField("Plaintext", "`" + decodedText + "`")
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
