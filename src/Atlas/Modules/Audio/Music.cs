using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Audio;
using System.Diagnostics;

namespace Atlas.Modules.Audio
{
    public class Music : ModuleBase<ICommandContext>
    {
        private readonly AudioService _service;
        public Music(AudioService service)
        {
            _service = service;
        }

        [RequireContext(ContextType.Guild)]
        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayVoice([Remainder] string song)
        {
            // To Do: Configurable Music Channel
            // To Do: Only JoinAudio() if current Voice Channel = null
            //IAudioClient client =  await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);

            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            var output = CreateStream(song).StandardOutput.BaseStream;
            var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024);
            output.CopyToAsync(stream);
            stream.FlushAsync().ConfigureAwait(false);
        }

        [RequireOwner]
        [RequireContext(ContextType.Guild)]
        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveVoice()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        private Process CreateStream(string song)
        {
            Process currentsong = new Process();

            currentsong.StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C youtube-dl.exe -o - " + song + " | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            currentsong.Start();
            return currentsong;
        }
    }
}