using Depot.Enitities;
using Discord;
using Discord.Commands;
using System.Text;

namespace Depot
{
    public class Warning : ModuleBase<SocketCommandContext>
    {
        [RequireUserPermission(GuildPermission.BanMembers)]
        [Command("badboi")]
        public async Task Wrn(IUser user, string arg)
        {
            if (user.IsBot) return;
            if (user.Id != 627015977233678336) return;
            Badboi badboi = Badbois.Ins.GetOrAdd(user.Id);
            badboi.Sacredtexts.Add(new(DateTime.Now, arg));
            Badbois.Ins.Save();
            await ReplyAsync("wawned");

            switch (badboi.Sacredtexts.Count)
            {
                case 3:
                    {
                        await Context.Guild.AddBanAsync(user);
                        await Task.Delay(5000);
                        await Context.Guild.RemoveBanAsync(user);
                    }
                    break;

                case 5:
                    {
                        await Context.Guild.AddBanAsync(user);
                    }
                    break;
            }
        }

        [Command("warns")]
        public async Task DisplayWarns(IUser user)
        {
            Badboi? badboi = Badbois.Ins.Get(user);
            if (badboi is null)
            {
                await ReplyAsync("im a goodboi, FOR NOW");
                return;
            }

            StringBuilder sb = new();
            sb.AppendLine($"{user} has warings {badboi.Sacredtexts.Count}:");
            foreach (var sacredtext in badboi.Sacredtexts)
            {
                sb.AppendLine($"{sacredtext.Timestamp}: {sacredtext.Comment}");
            }

            await ReplyAsync(sb.ToString());
        }
    }
}