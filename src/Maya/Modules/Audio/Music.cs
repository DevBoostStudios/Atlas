using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Maya.Modules.Audio
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
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel); // To Do: Configurable Music Channel
            await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        }

        [RequireOwner]
        [RequireContext(ContextType.Guild)]
        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveVoice()
        {
            await _service.LeaveAudio(Context.Guild);
        }
    }
}
