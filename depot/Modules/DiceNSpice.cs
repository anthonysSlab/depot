using Discord.Commands;
using System.Text;

namespace Depot.Modules
{
    public class DiceNSpice : ModuleBase<SocketCommandContext>
    {
        [Command("roll")]
        public async Task Roll(string arg)
        {
            /*if (Context.User.Id == 191521298772525056)
            {
                await ReplyAsync("f u jan");
                await Context.Message.DeleteAsync();
                return;
            };  //blocks jan from fucking around!*/
            if (Context.Guild is null && Context.User.IsBot) { await ReplyAsync("nyo bots ow dm's"); return; }; //no dm's or bots
            if (arg.StartsWith("d")) { arg = "1" + arg; };
            string[] allowedDice = { "2", "4", "6", "8", "10", "12", "20", "100" };
            string[] args = arg.Split("d");
            if (args.Length != 2) return;
            string value = args[1];
            int index = Array.IndexOf(allowedDice, value);
            if (!int.TryParse(args[0], out int hownamydices)) return; if (!int.TryParse(args[1], out int sides)) return; //turn to int
            int thegreatnum = 0;
            Random rand = new Random(); //random function
            if (index > -1) //only accept 2,4,6,8,10,12,100 as sides
            {
                if (hownamydices <= 10) //prevent more than 10 dice to be rolled at a time
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hownamydices; i++)
                    {
                        int derandomnumba = rand.Next(1, sides);
                        thegreatnum += derandomnumba;
                        sb.Append($"\n{derandomnumba}");
                    }
                    await ReplyAsync(sb.ToString());

                    if (hownamydices > 1)
                    {
                        await ReplyAsync($"TOTAL:{thegreatnum}");
                    }
                }
                else
                {
                    await ReplyAsync("woww wess dice ya bastewd");
                }
            }
            else { await ReplyAsync("ffat's nyot a dice ya bastewd"); };
        }
    }
}