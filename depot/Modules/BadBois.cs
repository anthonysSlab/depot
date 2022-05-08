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

        [RequireUserPermission(GuildPermission.BanMembers)]
        [Command("warn")]
        public async Task Wrn(IUser duser, string arg)
        {
            Guild? guild = _service.Context.GetGuild(Context.Guild);
            if (guild == null)
            {
                await ReplyAsync("Guild is not in the database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            Warning warning = new(duser.Id, arg, DateTime.Now);
            guild.Warnings.Add(warning);

            _service.Context.Guilds.Update(guild);
            await _service.Context.SaveChangesAsync();

            await ReplyAsync("wawned");

            int count = guild.Warnings.Count(x => x.UserId == duser.Id);

            IGuildUser? guser = Context.Guild.GetUser(duser.Id);

            if (guser == null)
            {
                await ReplyAsync("User is not on guild cannot punish");
                return;
            }

            switch (count)
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
                        await guser.SetTimeOutAsync(TimeSpan.FromDays(7));
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
        [Command("warn")]
        public async Task Wrn(ulong id, string arg)
        {
            Guild? guild = _service.Context.GetGuild(Context.Guild);
            if (guild == null)
            {
                await ReplyAsync("Guild is not in the database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            Warning warning = new(id, arg, DateTime.Now);
            guild.Warnings.Add(warning);

            _service.Context.Guilds.Update(guild);
            await _service.Context.SaveChangesAsync();

            await ReplyAsync("wawned");

            int count = guild.Warnings.Count(x => x.UserId == id);

            IGuildUser? guser = Context.Guild.GetUser(id);

            if (guser == null)
            {
                await ReplyAsync("User is not on guild cannot punish");
                return;
            }

            switch (count)
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
                        await guser.SetTimeOutAsync(TimeSpan.FromDays(7));
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
            Guild? guild = _service.Context.GetGuild(Context.Guild);
            if (guild == null)
            {
                await ReplyAsync("Guild is not in the database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            Warning? warning = guild.Warnings.Where(x => x.UserId == duser.Id).FirstOrDefault(x => x.Message == arg);

            if (warning == null)
            {
                await ReplyAsync("User or warning reason not found");
                return;
            }

            guild.Warnings.Remove(warning);

            _service.Context.Guilds.Update(guild);
            await _service.Context.SaveChangesAsync();
        }

        [RequireUserPermission(GuildPermission.BanMembers)]
        [Command("unwarn")]
        public async Task WarnRemove(ulong id, string arg)
        {
            Guild? guild = _service.Context.GetGuild(Context.Guild);
            if (guild == null)
            {
                await ReplyAsync("Guild is not in the database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            Warning? warning = guild.Warnings.Where(x => x.UserId == id).FirstOrDefault(x => x.Message == arg);

            if (warning == null)
            {
                await ReplyAsync("User or warning reason not found");
                return;
            }

            guild.Warnings.Remove(warning);

            _service.Context.Guilds.Update(guild);
            await _service.Context.SaveChangesAsync();
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("warns")]
        public async Task DisplayWarns(IUser duser)
        {
            Guild? guild = _service.Context.GetGuild(Context.Guild);
            if (guild == null)
            {
                await ReplyAsync("Guild is not in the database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            List<Warning> warnings = guild.Warnings.Where(x => x.UserId == duser.Id).ToList();
            StringBuilder sb = new();
            sb.AppendLine($"{duser} has warings {warnings.Count}:");
            foreach (var warning in warnings)
            {
                sb.AppendLine($"{warning.Timestamp}: {warning.Message}");
            }

            await ReplyAsync(sb.ToString());
        }

        [RequireUserPermission(GuildPermission.ManageMessages)]
        [Command("warns")]
        public async Task DisplayWarns(ulong id)
        {
            Guild? guild = _service.Context.GetGuild(Context.Guild);
            if (guild == null)
            {
                await ReplyAsync("Guild is not in the database, please report to my mom or to https://github.com/anthonysSlab/depot/issues");
                return;
            }

            List<Warning> warnings = guild.Warnings.Where(x => x.UserId == id).ToList();
            StringBuilder sb = new();
            sb.AppendLine($"{id} has {warnings.Count} warings:");
            foreach (var warning in warnings)
            {
                sb.AppendLine($"{warning.Timestamp}: {warning.Message}");
            }

            await ReplyAsync(sb.ToString());
        }
    }
}