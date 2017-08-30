﻿using Discord;
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
        [Summary("Base64 is a group of similar binary-to-text encoding schemes that represent binary data in an ASCII string format by translating it into a radix-64 representation.")]
        public class Base64 : ModuleBase<SocketCommandContext>
        {
            [Command("encode")]
            [Summary("Encode the specified Text in Base64.")]
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
                var decodedCipher = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                var builder = new EmbedBuilder()
                    .WithColor(new Color(5025616))
                    .WithAuthor(author =>
                    {
                        author
                        .WithName("Base64")
                        .WithIconUrl("http://i.imgur.com/ONH5AnP.png");
                    })
                    .AddField("Cipher Text", "`" + cipher + "`")
                    .AddField("Plaintext", "`" + decodedCipher + "`")
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
                    .AddField("Cipher Text", "`" + cipher + "`")
                    .AddField("Plaintext", "`" + decodedCipher + "`")
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

    }
}