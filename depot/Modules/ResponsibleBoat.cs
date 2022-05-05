using Depot.Enitities;
using Discord.Commands;
using Discord.WebSocket;

namespace Depot.Modules
{
    public class ResponsibleBoat : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        public async Task Say(params string[] arg)
        {
            string shit = string.Join(" ", arg);
            if (Context.User.Id == 627015977233678336) //only work me
            {
                await ReplyAsync(shit);
                await Context.Message.DeleteAsync();
            }
            else
            {
                await ReplyAsync("no");
            }
        }

        [Command("uwuify")]
        public async Task Uwuify(params string[] arg)
        {
            string uwunes = string.Join(" ", arg);
            if (Context.User.Id == 627015977233678336) { await Context.Message.DeleteAsync(); }
            if (string.IsNullOrEmpty(uwunes)) { await ReplyAsync("wewy funny"); return; }
            await base.ReplyAsync(Uwuifyer.UwuifyText(uwunes));
        }
    }
}