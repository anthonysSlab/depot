namespace Depot.Modules
{
    using Depot.Enitities;
    using Depot.Services;
    using Depot.TaskScheduler;
    using Discord;
    using Discord.Commands;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SacredScrolls : ModuleBase<SocketCommandContext>
    {
        private readonly ModerationService _service;
        private readonly ManualTrigger migrationTrigger;

        public SacredScrolls(ModerationService service)
        {
            _service = service;
            migrationTrigger = new();
            _service.MigrationTask.AddTrigger(migrationTrigger);
        }

        [RequireUserPermission(GuildPermission.ManageWebhooks)]
        [Command("sacredscrolls")]
        public async Task Enable()
        {
            ulong id = Context.Guild.Id;

            if (_service.Context.Guilds.Any(x => x.Id == id))
            {
                await ReplyAsync("already..?");
                return;
            }

            Guild guild = new() { Id = id };
            _service.Context.Guilds.Add(guild);
            await _service.Context.SaveChangesAsync();
            await ReplyAsync("will do");
        }

        [RequireUserPermission(GuildPermission.ManageWebhooks)]
        [Command("profanescrolls")]
        public async Task Disable()
        {
            ulong id = Context.Guild.Id;

            Guild? guild = _service.Context.Guilds.FirstOrDefault(x => x.Id == id);
            if (guild == null)
            {
                await ReplyAsync("already..?");
                return;
            }

            _service.Context.Guilds.Remove(guild);
            await _service.Context.SaveChangesAsync();
            await ReplyAsync("will do");
        }

        [RequireUserPermission(GuildPermission.ManageWebhooks)]
        [Command("godmode")]
        public async Task AddIgnoreRole(string roleName)
        {
            Guild? guild = _service.Context.Guilds.Include(g => g.Users).Include(g => g.IgnoredRoles).FirstOrDefault(x => x.Id == Context.Guild.Id);
            if (guild == null)
            {
                await ReplyAsync("it's disabled just like yer mom, you imbecile!");
                return;
            }

            IRole? drole = Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName);

            if (drole == null)
            {
                await ReplyAsync("do I really have to tell ye how stupid ye are?");
                return;
            }

            Role role = new() { Id = drole.Id };

            if (guild.IgnoredRoles.Any(r => r.Id == role.Id))
            {
                await ReplyAsync("they already immune, leave em alone!");
                return;
            }

            guild.IgnoredRoles.Add(role);
            _service.Context.Roles.Add(role);
            _service.Context.Guilds.Update(guild);
            try
            {
                await _service.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
#if DEBUG
                await ReplyAsync(ex.Message);
#endif
            }

            await ReplyAsync($"role {drole.Mention} added, hope ye happy now");
        }

        [RequireUserPermission(GuildPermission.ManageWebhooks)]
        [Command("devilmode")]
        public async Task RemoveIgnoreRole(string roleName)
        {
            Guild? guild = _service.Context.Guilds.Include(g => g.Users).Include(g => g.IgnoredRoles).FirstOrDefault(x => x.Id == Context.Guild.Id);
            if (guild == null)
            {
                await ReplyAsync("it's disabled just like yer mom, you imbecile!");
                return;
            }

            IRole? drole = Context.Guild.Roles.FirstOrDefault(x => x.Name == roleName);

            if (drole == null)
            {
                await ReplyAsync("do I really have to tell ye how stupid ye are?");
                return;
            }

            Role? role = guild.IgnoredRoles.FirstOrDefault(x => x.Id == drole.Id);

            if (role == null)
            {
                await ReplyAsync("they already fucked, leave em alone!");
                return;
            }

            guild.IgnoredRoles.Remove(role);
            _service.Context.Roles.Remove(role);
            _service.Context.Guilds.Update(guild);
            try
            {
                await _service.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
#if DEBUG
                await ReplyAsync(ex.Message);
#endif
            }

            await ReplyAsync($"role {drole.Mention} removed, hope ye happy now");
        }

        [RequireUserPermission(GuildPermission.ManageWebhooks)]
        [Command("markedtime")]
        public async Task SetWarnDuration(TimeSpan span)
        {
            Guild? guild = _service.Context.Guilds.Include(g => g.Users).Include(g => g.IgnoredRoles).FirstOrDefault(x => x.Id == Context.Guild.Id);
            if (guild == null)
            {
                await ReplyAsync("it's disabled just like yer mom, you imbecile!");
                return;
            }

            guild.DurationWarn = span;
            _service.Context.Guilds.Update(guild);
            _service.Context.SaveChanges();
            await ReplyAsync($"Set warn duration to {span}");
        }

        [RequireUserPermission(GuildPermission.ManageWebhooks)]
        [Command("fuckedtime")]
        public async Task SetKickDuration(TimeSpan span)
        {
            Guild? guild = _service.Context.Guilds.Include(g => g.Users).Include(g => g.IgnoredRoles).FirstOrDefault(x => x.Id == Context.Guild.Id);
            if (guild == null)
            {
                await ReplyAsync("it's disabled just like yer mom, you imbecile!");
                return;
            }

            guild.DurationKick = span;
            _service.Context.Guilds.Update(guild);
            _service.Context.SaveChanges();
            await ReplyAsync($"Set kick duration to {span}");
        }

        [Command("pod do your homework")]
        public async Task ForceClean()
        {
            if (Context.User.Id == 308203742736678914)
            {
                await migrationTrigger.TriggerAsync();
                await ReplyAsync("yes mommy");
            }
            else if (Context.User.Id == 627015977233678336)
            {
                await migrationTrigger.TriggerAsync();
                await ReplyAsync("yes daddy");
            }
            else
            {
                await ReplyAsync("I don't know you stop texting me, or I will tell it my mom!");
                await Context.Client.GetUser(308203742736678914).SendMessageAsync($"{Context.User.Mention} tried to execute {Context.Message.CleanContent}");
            }
        }

        [RequireUserPermission(GuildPermission.ManageWebhooks)]
        [Command("sacredinfo")]
        public async Task Info()
        {
            Guild? guild = _service.Context.Guilds.Include(g => g.Users).Include(g => g.IgnoredRoles).FirstOrDefault(x => x.Id == Context.Guild.Id);
            if (guild == null)
            {
                await ReplyAsync("your guild is not yet migrated, please wait a few minutes usually it takes 5min max!");
                return;
            }

            StringBuilder sb = new();

            sb.AppendLine(guild.ActivityKicking ? "auto kicking is enabled" : "auto kicking is disabled");
            sb.AppendLine($"Warn delay:{guild.DurationWarn}");
            sb.AppendLine($"Kick delay:{guild.DurationKick}");
            sb.AppendLine("ignored roles:");
            foreach (var role in guild.IgnoredRoles)
            {
                sb.AppendLine(Context.Guild.GetRole(role.Id).Name);
            }
            sb.AppendLine("tracked users:");
            foreach (var user in guild.Users)
            {
                sb.AppendLine($"{Context.Guild.GetUser(user.User.Id).DisplayName}, Last active:{user.LastActivity}, Warned:{user.HasActivityWarn}");
            }

            if (sb.Length > 2000)
            {
                string msg = sb.ToString();
                int position = 0;
                while (position < sb.Length)
                {
                    int len = position + 2000 > sb.Length ? sb.Length - position : 2000;
                    await ReplyAsync(msg.Substring(position, len));
                    position += len;
                }
            }
            else
                await ReplyAsync(sb.ToString());
        }
    }
}