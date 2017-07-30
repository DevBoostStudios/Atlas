using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System;
using System.Diagnostics;

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
            var builder = new EmbedBuilder()
                .WithAuthor(author =>
                {
                    author
                    .WithName("Music")
                    .WithIconUrl("https://cdn.discordapp.com/avatars/320328599603249156/33a1d01fc3af4aa5cdf54c1443d84047.webp");
                })
                .WithColor(new Color(5025616))
                .AddField("Session", "Time") // To Do: Voice Session Time
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