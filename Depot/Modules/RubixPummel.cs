using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Depot.Enitities;

namespace Depot.Modules
{
    public class RubixPummel : ModuleBase<SocketCommandContext>
    {
        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [Command("gam")]
        public async Task SetGam(params string[] game)
        {
            string text = string.Join(" ", game);

            await Context.Client.SetGameAsync(text);
            await ReplyAsync($"now pwaying: {text}");
        }
    }
}