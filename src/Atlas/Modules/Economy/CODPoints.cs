using Atlas.Data;
using Discord;
using Discord.Commands;
using LiteDB;
using System;
using System.Threading.Tasks;

namespace Atlas.Modules.Economy
{
    [Group("balance")]
    public class CODPoints : ModuleBase<SocketCommandContext>
    {
        // Property will be filled at runtime by the IoC container (Program.cs:49)
        public LiteDatabase Database { get; set; }
        
        [RequireContext(ContextType.Guild)]
        [Command]
        [Summary("Access a user's CODPoints Balance.")]
        public Task UserAsync(IUser user)
            => SendCODPointsAsync(user);

        private async Task SendCODPointsAsync(IUser user)
        {
            var users = Database.GetCollection<User>("users");
            var model = users.FindOne(u => u.Id == user.Id);
            var CODPoints = model?.Points ?? 0;

            var builder = new EmbedBuilder()
                .WithColor(new Color(5025616))
                .WithAuthor(author =>
                {
                    author
                    .WithName(Context.User.ToString())
                    .WithIconUrl(Context.User.GetAvatarUrl());
                })
                .AddInlineField("Balance", CODPoints + "CP")
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