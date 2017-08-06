using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Discord;
using Discord.Audio;

namespace Atlas.Modules.Audio
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

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
                // Logging stuffs
            }
        }

        public async Task LeaveVoice(IGuild guild)
        {
            IAudioClient client;
            if (ConnectedChannels.TryRemove(guild.Id, out client))
            {
                await client.StopAsync();
            }
        }

        //public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string song)
        //{
        //    string path = "Data/Temp/Audio/" + song + ".wav";

        //    if (!File.Exists(path))
        //    {
        //        await channel.SendMessageAsync("Error: File not found.");
        //        return;
        //    }

        //    IAudioClient client;
        //    if (ConnectedChannels.TryGetValue(guild.Id, out client))
        //    {
        //        // await Log(LogSeverity.Debug, "Starting playback of " + path + " in " + guild.Name + "");
        //        var output = CreateStream(path).StandardOutput.BaseStream;

        //        // Additional argument to CreatePCMStream() changes bitrate, defaults to 96 * 1024
        //        var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024, bufferMillis: 500);
        //        await output.CopyToAsync(stream);
        //        await stream.FlushAsync().ConfigureAwait(false);
        //    }
        //}

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
    }
}