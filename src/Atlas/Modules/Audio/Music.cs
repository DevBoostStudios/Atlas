using Discord;
using Discord.Audio;
using Discord.Commands;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Atlas.Modules.Audio
{
    public class Music : ModuleBase<ICommandContext>
    {
        private IConfiguration _config;

        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        [RequireContext(ContextType.Guild)]
        [Command("play", RunMode = RunMode.Async)]
        [Summary("To Do")]
        public async Task PlayAudio([Remainder] string song)
        {
            // To Do: Configurable Music Channel
            // To Do: Only JoinVoice() if current connected voice channel = null
            await JoinVoice(Context.Guild, (Context.User as IVoiceState).VoiceChannel);

            if (Uri.IsWellFormedUriString(song, UriKind.Absolute))
            {
                await SendAudioAsync(Context.Guild, Context.Channel, song);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    _config = BuildConfig();

                    var json = await client.GetStringAsync("https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=1&safeSearch=none&type=video&q=" + song + "&key=" + _config["googleKey"]);
                    dynamic parse = JsonConvert.DeserializeObject(json);

                    string songURL = "https://www.youtube.com/watch?v=" + parse.items[0].id.videoId;

                    await SendAudioAsync(Context.Guild, Context.Channel, songURL);
                }
            }
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireContext(ContextType.Guild)]
        [Command("stop", RunMode = RunMode.Async)]
        [Summary("To Do")]
        public async Task StopAudio()
        {
            IAudioClient client;
            if (ConnectedChannels.TryRemove(Context.Guild.Id, out client))
            {
                await client.StopAsync();
            }
        }

        public async Task JoinVoice(IGuild guild, IVoiceChannel target)
        {
            IAudioClient client;
            if (ConnectedChannels.TryGetValue(guild.Id, out client))
            {
                return;
            }
            {
                if (target.Guild.Id != guild.Id)
                    return;
            }

            var audioClient = await target.ConnectAsync();

            if (ConnectedChannels.TryAdd(guild.Id, audioClient))
            {
                // await Log(LogSeverity.Info, "Connected to voice on " + guild.Name + ".");
            }
        }

        public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string song)
        {
            IAudioClient client;
            if (ConnectedChannels.TryGetValue(guild.Id, out client))
            {
                var output = CreateStream(song).StandardOutput.BaseStream;
                var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024);

                await output.CopyToAsync(stream);
                await stream.FlushAsync().ConfigureAwait(false);
            }
        }

        public Process CreateStream(string song)
        {
            Process audioStream = new Process();

            audioStream.StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C youtube-dl -o - " + song + " | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            audioStream.Start();
            return audioStream;
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