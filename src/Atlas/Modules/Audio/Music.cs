using Discord;
using Discord.Audio;
using Discord.Commands;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Atlas.Modules.Audio
{
    public class Music : ModuleBase<ICommandContext>
    {
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
                string songURL = song;
                await SendAudioAsync(Context.Guild, Context.Channel, songURL);
            }
            else
            {
                string songQuery = "ytsearch1:\"" + song + "\"";
                await SendAudioAsync(Context.Guild, Context.Channel, songQuery);
            }
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireContext(ContextType.Guild)]
        [Command("stop", RunMode = RunMode.Async)]
        [Summary("To Do")]
        public async Task StopAudio() // To Do: This doesn't work
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
                // await Log(LogSeverity.Info, "Connected to voice on " + guild.Name + "."); // Debug
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

        public Process CreateStream([Remainder] string song)
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
    }
}