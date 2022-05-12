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
        public async Task Wrn(IGuildUser duser, string arg)
        {
            User? user = _service.Context.Users.FirstOrDefault(x => x.Id == duser.Id);

            if (user == null)
            {
                await ReplyAsync("User is not in database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            Guild? guild = _service.Context.Guilds.FirstOrDefault(x => x.Id == duser.Guild.Id);

            if (guild == null)
            {
                await ReplyAsync("Guild is not in database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            Warning warning = new(user, guild, arg, DateTime.Now);
            user.Warnings.Add(warning);
            guild.Warnings.Add(warning);

            _service.Context.Users.Update(user);
            _service.Context.Guilds.Update(guild);
            await _service.Context.SaveChangesAsync();

            await ReplyAsync("wawned");

            switch (user.Warnings.Count)
            {
                case 1:
                    {
                        await duser.SetTimeOutAsync(TimeSpan.FromMinutes(10));
                    }
                    break;

                case 2:
                    {
                        await duser.SetTimeOutAsync(TimeSpan.FromHours(1));
                    }
                    break;

                case 3:
                    {
                        await duser.KickAsync();
                    }
                    break;

                case 4:
                    {
                        await duser.BanAsync();
                    }
                    break;
            }
        }

        [RequireUserPermission(GuildPermission.BanMembers)]
        [Command("unwarn")]
        public async Task WarnRemove(IGuildUser duser, string arg)
        {
            User? user = _service.Context.Users.FirstOrDefault(x => x.Id == duser.Id);

            if (user == null)
            {
                await ReplyAsync("User is not in database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            Guild? guild = _service.Context.Guilds.FirstOrDefault(x => x.Id == duser.Guild.Id);

            if (guild == null)
            {
                await ReplyAsync("Guild is not in database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            Warning? warning = guild.Warnings.FirstOrDefault(x => x.Message.StartsWith(arg) && x.UserId == duser.Id);

            if (warning == null)
            {
                await ReplyAsync("Warning not found");
                return;
            }

            user.Warnings.Remove(warning);
            guild.Warnings.Remove(warning);

            _service.Context.Users.Update(user);
            _service.Context.Guilds.Update(guild);
            await _service.Context.SaveChangesAsync();
        }

        [RequireUserPermission(GuildPermission.ViewAuditLog)]
        [Command("warns")]
        public async Task DisplayWarns(IGuildUser duser)
        {
            User? user = _service.Context.Users.FirstOrDefault(x => x.Id == duser.Id);

            if (user == null)
            {
                await ReplyAsync("User is not in database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            Guild? guild = _service.Context.Guilds.FirstOrDefault(x => x.Id == duser.Guild.Id);

            if (guild == null)
            {
                await ReplyAsync("Guild is not in database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            IEnumerable<Warning> warnings = user.Warnings.Where(x => x.GuildId == guild.Id);
            StringBuilder sb = new();
            sb.AppendLine($"{duser} has {warnings.Count()} warnings:");
            foreach (var warning in warnings)
            {
                sb.AppendLine($"{warning.Timestamp}: {warning.Message}");
            }

            await ReplyAsync(sb.ToString());
        }
    }
}