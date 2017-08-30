using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atlas.Modules.Utility
{
    public class Cryptography : ModuleBase<SocketCommandContext>
    {
        [Group("base64")]
        [Summary("Base64 is a group of similar binary-to-text encoding schemes that represent binary data in an ASCII string format by translating it into a radix-64 representation.")]
        public class Base64 : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("Encode the specified Text in Base64.")]
            public async Task Base64Encode([Remainder] string text)
            {
                var textBytes = Encoding.UTF8.GetBytes(text);
                var encodedText = Convert.ToBase64String(textBytes);

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Base64")
                        .WithIconUrl("http://i.imgur.com/ONH5AnP.png");
                    })
                    .AddField("Decoded", "`" + text + "`")
                    .AddField("Encoded", "`" + encodedText + "`")
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
                var decodedCipher = Encoding.UTF8.GetString(base64EncodedBytes);

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Base64")
                        .WithIconUrl("http://i.imgur.com/ONH5AnP.png");
                    })
                    .AddField("Encoded", "`" + cipher + "`")
                    .AddField("Decoded", "`" + decodedCipher + "`")
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

        [Group("binary")]
        [Summary("Binary code represents text using the binary number system's 0 and 1.")]
        public class Binary : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("Encode the specified Text in Binary.")]
            public async Task BinaryEncode([Remainder] string text)
            {
                var encodedText = ToBinary(ConvertToByteArray(text, Encoding.UTF8));

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Binary")
                        .WithIconUrl("http://i.imgur.com/ONH5AnP.png");
                    })
                    .AddField("Decoded", "`" + text + "`")
                    .AddField("Encoded", "`" + encodedText + "`")
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
            [Summary("Decode the specified Binary Ciphertext to Plaintext.")]
            public async Task BinaryDecode([Remainder] string cipher)
            {
                var cipherClean = cipher.Replace(" ", "");
                var binaryEncodedBytes = GetBytesFromBinaryString(cipherClean);
                var decodedCipher = Encoding.ASCII.GetString(binaryEncodedBytes);

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Binary")
                        .WithIconUrl("http://i.imgur.com/ONH5AnP.png");
                    })
                    .AddField("Encoded", "`" + cipher + "`")
                    .AddField("Decoded", "`" + decodedCipher + "`")
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

            public static byte[] ConvertToByteArray(string text, Encoding encoding)
            {
                return encoding.GetBytes(text);
            }

            public static String ToBinary(Byte[] data)
            {
                return string.Join(" ", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
            }

            public Byte[] GetBytesFromBinaryString(String binary)
            {
                var list = new List<Byte>();

                for (int i = 0; i < binary.Length; i += 8)
                {
                    String t = binary.Substring(i, 8);

                    list.Add(Convert.ToByte(t, 2));
                }

                return list.ToArray();
            }
        }

        [Group("hex")]
        [Summary("Hexadecimal is a positional numeral system with a radix, or base, of 16.")]
        public class Hex : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("Encode the specified Text in Hex.")]
            public async Task HexEncode([Remainder] string text)
            {
                var encodedText = string.Join("", text.Select(c => ((int)c).ToString("X2")));

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Hex")
                        .WithIconUrl("http://i.imgur.com/ONH5AnP.png");
                    })
                    .AddField("Decoded", "`" + text + "`")
                    .AddField("Encoded", "`" + encodedText + "`")
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
            [Summary("Decode the specified Hex Ciphertext to Plaintext.")]
            public async Task HexDecode([Remainder] string cipher)
            {
                var hexEncodedBytes = new byte[cipher.Length / 2];
                for (var i = 0; i < hexEncodedBytes.Length; i++)
                {
                    hexEncodedBytes[i] = Convert.ToByte(cipher.Substring(i * 2, 2), 16);
                }

                var decodedCipher = Encoding.UTF8.GetString(hexEncodedBytes);

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Hex")
                        .WithIconUrl("http://i.imgur.com/ONH5AnP.png");
                    })
                    .AddField("Encoded", "`" + cipher + "`")
                    .AddField("Decoded", "`" + decodedCipher + "`")
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

        [Group("md5")]
        [Summary("MD5 algorithm is a widely used hash function producing a 128-bit hash value.")]
        public class MD5 : ModuleBase<SocketCommandContext>
        {
            [Command("hash")]
            [Summary("Hash the specified Text using MD5.")]
            public async Task MD5Encode([Remainder] string text)
            {
                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] textBytes = Encoding.ASCII.GetBytes(text);
                    byte[] hashBytes = md5.ComputeHash(textBytes);

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    var hashedText = sb.ToString();

                    var builder = new EmbedBuilder()
                        .WithColor(new Color(5025616))
                        .WithAuthor(author =>
                        {
                            author
                            .WithName("MD5")
                            .WithIconUrl("http://i.imgur.com/ONH5AnP.png");
                        })
                        .AddField("Text", "`" + text + "`")
                        .AddField("Hash", "`" + hashedText + "`")
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

        [Group("morse")]
        [Summary("To Do")]
        public class Morse : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task MorseEncode([Remainder] string text)
            {
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task MorseDecode([Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
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
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task A1Z26Decode([Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
            }
        }

        [Group("caesar")]
        [Summary("To Do")]
        public class Caesar : ModuleBase<SocketCommandContext>
        { // https://www.dotnetperls.com/caesar
            [Command("encode")]
            [Summary("To Do")]
            public async Task CaesarEncode(int shift, [Remainder] string text)
            {
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task CaesarDecode(int shift, [Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
            }
        }

        [Group("caesar")]
        [Summary("To Do")]
        public class KeyedCaesar : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task CaesarEncode(int shift, string key, [Remainder] string text)
            {
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task CaesarDecode(int shift, string key, [Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
            }
        }

        [Group("aes")]
        [Summary("To Do")]
        public class AES : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task AESEncode(int bit, string key, [Remainder] string text)
            {
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task AESDecode(int bit, string key, [Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
            }
        }

        [Group("rsa")]
        [Summary("To Do")]
        public class RSA : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task RSAEncode(int prime, int prime2, [Remainder] string text)
            {
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task RSADecode(int prime, int prime2, [Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
            }
        }

        [Group("sha")]
        [Summary("To Do")]
        public class SHA : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task SHAEncode([Remainder] string text)
            {
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task SHADecode([Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
            }
        }

        [Group("atbash")]
        [Summary("To Do")]
        public class Atbash : ModuleBase<SocketCommandContext>
        { // https://www.dotnetperls.com/atbash
            [Command("encode")]
            [Summary("To Do")]
            public async Task AtbashEncode([Remainder] string text)
            {
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task AtbashDecode([Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
            }
        }

        [Group("gronsfeld")]
        [Summary("To Do")]
        public class Gronsfeld : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task GronsfeldEncode(string alphabet, string key, [Remainder] string text)
            {
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task GronsfeldDecode(string alphabet, string key, [Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
            }
        }

        [Group("pad")]
        [Summary("To Do")]
        public class Pad : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task PadEncode(string pad, [Remainder] string text)
            {
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task PadDecode(string pad, [Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
            }
        }

        [Group("rot13")]
        [Summary("To Do")]
        public class Rot13 : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task Rot13Encode([Remainder] string text)
            {
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task Rot13Decode([Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
            }
        }

        [Group("vignere")]
        [Summary("To Do")]
        public class Vignere : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task VignereEncode(string key, [Remainder] string text)
            {
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task VignereDecode(string key, [Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
            }
        }

        [Group("vignere")]
        [Summary("To Do")]
        public class KeyedVignere : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("To Do")]
            public async Task VignereEncode(string alphabet, string key, [Remainder] string text)
            {
                await ReplyAsync("Debug: To Do");
            }

            [Command("decode")]
            [Summary("To Do")]
            public async Task VignereDecode(string alphabet, string key, [Remainder] string cipher)
            {
                await ReplyAsync("Debug: To Do");
            }
        }
    }
}
