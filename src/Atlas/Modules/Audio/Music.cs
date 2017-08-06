using System.Threading.Tasks;
using Discord;
using Discord.Commands;

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
            await _service.JoinVoice(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
            await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
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