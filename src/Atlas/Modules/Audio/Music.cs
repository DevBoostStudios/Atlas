using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Audio;

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
        public async Task PlayAudio(string song)
        {
            // To Do: Configurable Music Channel
            // To Do: Only JoinVoice() if current connected voice channel = null
            //IAudioClient client = await _service.JoinVoice(Context.Guild, (Context.User as IVoiceState).VoiceChannel);

            IVoiceChannel channel = (Context.User as IVoiceState).VoiceChannel;
            IAudioClient client = await channel.ConnectAsync();

            var output = _service.CreateStream(song).StandardOutput.BaseStream;
            var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024);

            await output.CopyToAsync(stream);
            await stream.FlushAsync()
                .ConfigureAwait(false);
        }

        [RequireOwner]
        [RequireContext(ContextType.Guild)]
        [Command("stop", RunMode = RunMode.Async)]
        public async Task StopAudio()
        {
            await _service.LeaveVoice(Context.Guild);
        }
    }
}