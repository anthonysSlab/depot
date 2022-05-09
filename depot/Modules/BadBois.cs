using Depot.Enitities;
using Depot.Services;
using Discord;
using Discord.Commands;
using System.Text;

namespace Depot.Modules
{
    public class BadBois : ModuleBase<SocketCommandContext>
    {
        private readonly ModerationService _service;

        public BadBois(ModerationService service)
        {
            _service = service;
        }

        [RequireUserPermission(GuildPermission.KickMembers)]
        [Command("warn")]
        public async Task Wrn(IUser duser, string arg)
        {
            GuildUser? user = _service.Context.GuildUsers.FirstOrDefault(x => x.UserId == duser.Id);

            if (user == null)
            {
                await ReplyAsync("User not in database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            IGuildUser guser = Context.Guild.GetUser(user.UserId);

            user.Warnings.Add(new(user, arg, DateTime.Now));

            _service.Context.GuildUsers.Update(user);
            await _service.Context.SaveChangesAsync();

            await ReplyAsync("wawned");

            switch (user.Warnings.Count)
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

        [RequireUserPermission(GuildPermission.BanMembers)]
        [Command("unwarn")]
        public async Task WarnRemove(IUser duser, string arg)
        {
            GuildUser? user = _service.Context.GuildUsers.FirstOrDefault(x => x.UserId == duser.Id);

            if (user == null)
            {
                await ReplyAsync("User not in database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            user.Warnings.Remove(user.Warnings.First(x => x.Message == arg));

            _service.Context.GuildUsers.Update(user);
            await _service.Context.SaveChangesAsync();
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("warns")]
        public async Task DisplayWarns(IUser duser)
        {
            GuildUser? user = _service.Context.GuildUsers.FirstOrDefault(x => x.UserId == duser.Id);

            if (user == null)
            {
                await ReplyAsync("User not in database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            StringBuilder sb = new();
            sb.AppendLine($"{duser} has warings {user.Warnings.Count}:");
            foreach (var warning in user.Warnings)
            {
                sb.AppendLine($"{warning.Timestamp}: {warning.Message}");
            }

            await ReplyAsync(sb.ToString());
        }
    }
}