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
        [Command("gam")]
        public async Task SetGam(params string[] game)
        {
            string text = string.Join(" ", game);

            if (Context.User.Id == 627015977233678336)
            {
                await Context.Client.SetGameAsync(text);
                await ReplyAsync($"now pwaying: {text}");
            }
            else
            {
                await ReplyAsync("no");
            }
        }
    }
}