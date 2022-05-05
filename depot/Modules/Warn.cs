using Depot.Enitities;
using Discord;
using Discord.Commands;
using System.Text;

namespace Depot
{
    public class Warning : ModuleBase<SocketCommandContext>
    {
        [RequireUserPermission(GuildPermission.BanMembers)]
        [Command("warn")]
        public async Task Wrn(IUser user, string arg)
        {
            IGuildUser guser = Context.Guild.GetUser(user.Id);

            if (user.IsBot) return;
            Badboi badboi = Badbois.Ins.GetOrAdd(user.Id);
            badboi.Sacredtexts.Add(new(DateTime.Now, arg));
            Badbois.Ins.Save();
            await ReplyAsync("wawned");

            switch (badboi.Sacredtexts.Count)
            {
                case 1:
                    {
                        await guser.SetTimeOutAsync(TimeSpan.FromMinutes(10));
                    }
                    break;

                case 2:
                    {
                        await guser.SetTimeOutAsync(TimeSpan.FromHours(1));
                    }
                    break;

                case 3:
                    {
                        await guser.KickAsync();
                    }
                    break;

                case 4:
                    {
                        await guser.BanAsync();
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