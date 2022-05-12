namespace Depot.Services
{
    using Depot.Contexts;
    using Depot.Enitities;
    using Depot.TaskScheduler;
    using Discord;
    using Discord.WebSocket;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class ModerationService
    {
        private readonly DiscordSocketClient client;
        private readonly LogService log;
        private readonly ModerationContext context;
        private readonly ScheduledTask scheduledTask;

        public ModerationService(DiscordSocketClient client, LogService log)
        {
            this.client = client;
            this.log = log;
            context = new();
            context.Database.Migrate();
            client.Ready += Ready;
            client.LeftGuild += LeftGuild;
            client.JoinedGuild += JoinedGuild;
            client.UserJoined += UserJoined;
            client.UserLeft += UserLeft;
            client.RoleCreated += RoleCreated;
            client.RoleDeleted += RoleDeleted;
            client.GuildMemberUpdated += GuildMemberUpdated;

            scheduledTask = new(DoTasks);
            scheduledTask.AddTrigger(new TimeIntervalTrigger(TimeSpan.FromMinutes(1)));
            scheduledTask.Start();
        }

        public ScheduledTask MigrationTask => scheduledTask;

        private async Task Ready()
        {
            Task task = DoTasks();
            await task;
            if (task.IsFaulted)
                await log.LogAsync(new(LogSeverity.Error, nameof(ModerationService), "DoTasks failed", task.Exception));
        }

        private async Task GuildMemberUpdated(Cacheable<SocketGuildUser, ulong> arg1, SocketGuildUser arg2)
        {
            await UpdateUserRoles(arg2);
        }

        private async Task RoleCreated(SocketRole arg)
        {
            Guild? guild = context.Guilds.Find(arg.Guild.Id);
            if (guild == null) return;
            await EnsureCreatedRole(arg, guild);
        }

        private async Task RoleDeleted(SocketRole arg)
        {
            Guild? guild = context.Guilds.Find(arg.Guild.Id);
            if (guild == null) return;
            Role? role = guild.GetRole(arg);
            if (role == null) return;
            guild.Roles.Remove(role);
            context.Guilds.Update(guild);
            context.Roles.Remove(role);
            await context.SaveChangesAsync();
        }

        private async Task UserJoined(SocketGuildUser arg)
        {
            Guild? guild = context.Guilds.Find(arg.Guild.Id);
            if (guild == null) return;
            await EnsureCreatedUser(arg, guild);
        }

        private async Task UserLeft(SocketGuild arg1, SocketUser arg2)
        {
            Guild? guild = context.Guilds.Find(arg1.Id);
            if (guild == null) return;
            GuildUser? guildUser = guild.GetGuildUser(arg2);
            if (guildUser == null) return;
            User user = guildUser.User;
            guild.Users.Remove(guildUser);
            user.Guilds.Remove(guildUser);
            context.Guilds.Update(guild);
            context.Users.Update(user);
            context.GuildUsers.Remove(guildUser);

            if (user.Guilds.Count == 0)
            {
                context.Users.Remove(user);
            }

            await context.SaveChangesAsync();
        }

        private async Task JoinedGuild(SocketGuild arg)
        {
            await EnsureCreatedGuild(arg);
            await UpdateRoles(arg);
            await UpdateUsers(arg);
        }

        private async Task LeftGuild(SocketGuild arg)
        {
            Guild? guild = context.Guilds.Find(arg.Id);
            if (guild == null) return;

            context.Guilds.Remove(guild);
            await context.SaveChangesAsync();
        }

        public async Task<User> AddOrGetUser(ulong id)
        {
            User? user = context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                user = new(id);
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
            }

            return user;
        }

        private async Task MessageReceived(SocketMessage arg)
        {
            // tries to get the guild from the author.
            IGuild? dguild = (arg.Author as IGuildUser)?.Guild;

            // if the guild is not accessable we can't track the user so just return.
            if (dguild == null) return;

            // tries to find the guild in the database if it is enabled on the guild we get a instance back.
            Guild? guild = context.Guilds.FirstOrDefault(x => x.Id == dguild.Id);

            // if the guild is not in the database just skip the message.
            if (guild == null) return;

            // tries to get the user from the database it could be that the user is so new that the user is not yet tracked.
            GuildUser? user = guild.Users?.FirstOrDefault(x => x.User.Id == arg.Author.Id);

            // if the user is not tracked skip.
            if (user == null) return;

            // sets the last activity to now utc.
            user.LastActivity = DateTime.UtcNow;

            // removes active activity warnings.
            user.HasActivityWarn = false;

            // sets the warning time to default.
            user.ActivityWarn = default;

            // updates the guild in the database.
            context.Guilds.Update(guild);

            // commit changes to the database.
            Task task = context.SaveChangesAsync();
            await task;
            if (task.IsFaulted)
            {
                await log.LogAsync(new(LogSeverity.Warning, "Activity Update Task", "Updating the database failed", task.Exception));
            }
        }

        public ModerationContext Context => context;

        public async Task DoTasks()
        {
            await UpdateGuilds();

            await UpdateRoles();

            await UpdateUsers();

            await UpdateUserRoles();

            await UpdateInactivities();
        }

        #region Update methods

        public async Task UpdateInactivities()
        {
            List<Guild> removeQueue = new();
            foreach (Guild guild in context.Guilds)
            {
                Task task = UpdateInactivities(guild);
                await task;
                if (task.IsFaulted)
                {
                    removeQueue.Add(guild);
                    await log.LogAsync(new LogMessage(LogSeverity.Warning, nameof(UpdateInactivities), task.Exception?.ToString()));
                }
            }

            context.Guilds.RemoveRange(removeQueue);
            await context.SaveChangesAsync();
        }

        public async Task UpdateInactivities(Guild guild)
        {
            if (!guild.ActivityKicking)
                return;

            SocketGuild? dguild = client.GetGuild(guild.Id);

            // searches for inactive users.
            foreach (GuildUser user in guild.Users)
            {
                IGuildUser guser = dguild.GetUser(user.User.Id);

                // checks if the user is a bot and if the user is getting tracked else we just skip the user.
                if (guser.IsBot || guser.RoleIds.Any(x => guild.IgnoredRoles.Any(y => y.RoleId == x)))
                {
                    continue;
                }

                // if the user has no activity after the given time and the user has no warn, the user will be warned.
                if (user.LastActivity + guild.DurationWarn < DateTime.UtcNow && !user.HasActivityWarn)
                {
                    await guser.SendMessageAsync("ye wiww be wemoved fwom ffe gwain of sawt sewvew fow inactivity in 5 days; to awoid ffis pwease type smff in chat");
                    user.HasActivityWarn = true;
                    user.ActivityWarn = DateTime.UtcNow;
                    context.GuildUsers.Update(user);
                    continue;
                }

                // if the user is still after the warning not active then we kick the user.
                if (user.ActivityWarn + guild.DurationKick < DateTime.UtcNow && user.HasActivityWarn)
                {
                    await guser.KickAsync("inactivity");
                    continue;
                }
            }

            // saves the changes.
            await context.SaveChangesAsync();
        }

        public async Task UpdateGuilds()
        {
            HashSet<ulong> lguilds = context.Guilds.Select(x => x.Id).ToHashSet();
            HashSet<ulong> rguilds = client.Guilds.Select(x => x.Id).ToHashSet();

            foreach (IGuild guild in client.Guilds)
            {
                if (!lguilds.Contains(guild.Id))
                    await EnsureCreatedGuild(guild);
            }

            List<Guild> removeQueue = new();
            foreach (Guild guild in context.Guilds)
            {
                if (!rguilds.Contains(guild.Id))
                    removeQueue.Add(guild);
            }

            context.Guilds.RemoveRange(removeQueue);
            await context.SaveChangesAsync();
        }

        public async Task EnsureCreatedGuild(IGuild rguild)
        {
            Guild? guild = await context.Guilds.FindAsync(rguild.Id);
            if (guild == null)
            {
                guild = new(rguild.Id);
                context.Guilds.Add(guild);
            }
        }

        public async Task UpdateRoles()
        {
            foreach (IGuild guild in client.Guilds)
                await UpdateRoles(guild);
        }

        public async Task UpdateRoles(IGuild rguild)
        {
            Guild? guild = await context.Guilds.FindAsync(rguild.Id);
            if (guild == null) throw new ArgumentException(null, nameof(rguild));

            HashSet<ulong> lroles = guild.Roles.Select(x => x.Id).ToHashSet();
            HashSet<ulong> rroles = rguild.Roles.Select(x => x.Id).ToHashSet();

            foreach (IRole role in rguild.Roles)
            {
                if (!lroles.Contains(role.Id))
                    await EnsureCreatedRole(role, guild);
            }

            List<Role> removeQueue = new();
            foreach (Role role in guild.Roles)
            {
                if (!rroles.Contains(role.Id))
                    removeQueue.Add(role);
            }

            context.Roles.RemoveRange(removeQueue);
            await context.SaveChangesAsync();
        }

        public async Task EnsureCreatedRole(IRole rrole, Guild guild)
        {
            Role? role = await context.Roles.FindAsync(rrole.Id);
            if (role == null)
            {
                role = new(rrole.Id, guild);
                guild.Roles.Add(role);
                context.Roles.Add(role);
                context.Guilds.Update(guild);
            }
        }

        public async Task UpdateUsers()
        {
            foreach (SocketGuild guild in client.Guilds)
                await UpdateUsers(guild);
        }

        public async Task UpdateUsers(SocketGuild rguild)
        {
            Guild? guild = await context.Guilds.FindAsync(rguild.Id);
            if (guild == null) throw new ArgumentException(null, nameof(rguild));

            await rguild.DownloadUsersAsync();

            HashSet<ulong> lusers = guild.Users.Select(x => x.UserId).ToHashSet();
            HashSet<ulong> rusers = rguild.Users.Select(x => x.Id).ToHashSet();

            foreach (IGuildUser user in rguild.Users)
            {
                if (!lusers.Contains(user.Id))
                    await EnsureCreatedUser(user, guild);
            }

            List<GuildUser> removeQueue = new();
            foreach (GuildUser user in guild.Users)
            {
                if (!rusers.Contains(user.UserId))
                    removeQueue.Add(user);
            }

            context.GuildUsers.RemoveRange(removeQueue);
            await context.SaveChangesAsync();
        }

        public async Task EnsureCreatedUser(IGuildUser ruser, Guild guild)
        {
            GuildUser? guildUser = guild.Users.Find(x => x.UserId == ruser.Id);
            if (guildUser == null)
            {
                User? user = context.Users.Find(ruser.Id);
                if (user == null)
                {
                    user = new(ruser.Id);
                    context.Users.Add(user);
                    await context.SaveChangesAsync();
                }

                guildUser = new(user, guild, DateTime.UtcNow);
                user.Guilds.Add(guildUser);
                guild.Users.Add(guildUser);
                context.GuildUsers.Add(guildUser);
                context.Users.Update(user);
                context.Guilds.Update(guild);
                await context.SaveChangesAsync();
            }

            HashSet<ulong> luroles = guildUser.Roles.Select(x => x.Id).ToHashSet();

            foreach (ulong roleId in ruser.RoleIds)
            {
                if (!luroles.Contains(roleId))
                    EnsureGuildUserHasRole(roleId, guildUser);
            }
        }

        public void EnsureGuildUserHasRole(ulong roleid, GuildUser guildUser)
        {
            Role role = context.Roles.Find(roleid) ?? throw new ArgumentOutOfRangeException(nameof(roleid));
            guildUser.Roles.Add(role);
            role.Users.Add(guildUser);
            context.GuildUsers.Update(guildUser);
        }

        public async Task UpdateUserRoles()
        {
            foreach (SocketGuild guild in client.Guilds)
                await UpdateUserRoles(guild);
        }

        public async Task UpdateUserRoles(SocketGuild rguild)
        {
            Guild guild = context.Guilds.Find(rguild.Id) ?? throw new ArgumentOutOfRangeException(nameof(rguild));
            foreach (IGuildUser guildUser in rguild.Users)
                await UpdateUserRoles(guildUser, guild);
        }

        public async Task UpdateUserRoles(IGuildUser ruser)
        {
            Guild guild = context.Guilds.Find(ruser.Guild.Id) ?? throw new ArgumentOutOfRangeException(nameof(ruser));
            await UpdateUserRoles(ruser, guild);
        }

        public async Task UpdateUserRoles(IGuildUser ruser, Guild guild)
        {
            GuildUser guildUser = guild.GetGuildUser(ruser.Id) ?? throw new ArgumentOutOfRangeException(nameof(ruser));

            HashSet<ulong> luroles = guildUser.Roles.Select(x => x.Id).ToHashSet();
            HashSet<ulong> ruroles = ruser.RoleIds.ToHashSet();

            foreach (ulong roleId in ruser.RoleIds)
            {
                if (!luroles.Contains(roleId))
                    EnsureGuildUserHasRole(roleId, guildUser);
            }

            List<Role> removeQueue = new();
            foreach (Role role in guildUser.Roles)
            {
                if (!ruroles.Contains(role.Id))
                    removeQueue.Add(role);
            }

            foreach (Role role in removeQueue)
            {
                guildUser.Roles.Remove(role);
            }

            context.GuildUsers.Update(guildUser);
            await context.SaveChangesAsync();
        }

        #endregion Update methods
    }
}