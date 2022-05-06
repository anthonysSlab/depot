using Depot.Enitities;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Depot.Modules
{
    public class ResponsibleBoat : ModuleBase<SocketCommandContext>
    {
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("say")]
        public async Task Say(params string[] arg)
        {
            string shit = string.Join(" ", arg);
            await ReplyAsync(shit);
            await Context.Message.DeleteAsync();
        }

        [Command("uwuify")]
        public async Task Uwuify(params string[] arg)
        {
            string uwunes = string.Join(" ", arg);
            if (Context.Guild.GetUser(Context.User.Id).GuildPermissions.ManageMessages) { await Context.Message.DeleteAsync(); }
            if (string.IsNullOrEmpty(uwunes)) { await ReplyAsync("wewy funny"); return; }
            await base.ReplyAsync(Uwuifyer.UwuifyText(uwunes));
        }
    }
}