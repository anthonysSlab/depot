namespace Depot.Services
{
    using Depot.Contexts;
    using Depot.Enitities;
    using Depot.TaskScheduler;
    using Discord;
    using Discord.WebSocket;
    using System.Threading.Tasks;

    public class ModerationService
    {
        private readonly DiscordSocketClient _client;
        private readonly LogService log;
        private readonly ModerationContext _context;
        private readonly ScheduledTask scheduledTask;

        public ModerationService(DiscordSocketClient client, LogService log)
        {
            _client = client;
            this.log = log;
            _context = new();
            _client.MessageReceived += MessageReceived;
            _client.JoinedGuild += JoinedGuild;
            _client.LeftGuild += LeftGuild;
            _client.UserJoined += UserJoined;
            _client.UserLeft += UserLeft;

            scheduledTask = new(DoTasks);
            scheduledTask.AddTrigger(new TimeIntervalTrigger(TimeSpan.FromMinutes(5)));
            scheduledTask.Start();
            DoTasks().Wait();
        }

        private async Task UserLeft(SocketGuild arg1, SocketUser arg2)
        {
            GuildUser? guildUser = _context.GetGuildUser(arg1, arg2);
            if (guildUser == null) return;

            _context.GuildUsers.Remove(guildUser);
            await _context.SaveChangesAsync();
        }

        private async Task UserJoined(SocketGuildUser arg)
        {
            Guild? guild = _context.Guilds.FirstOrDefault(x => x.Id == arg.Guild.Id);
            if (guild == null)
            {
                return;
            }
            User user = await AddOrGetUser(arg.Id);
            GuildUser guildUser = new(user, guild, DateTime.UtcNow);
            guild.Users.Add(guildUser);
            user.Guilds.Add(guildUser);

            _context.GuildUsers.Add(guildUser);
            _context.Guilds.Update(guild);
            _context.Users.Update(user);

            await _context.SaveChangesAsync();
        }

        private async Task LeftGuild(SocketGuild arg)
        {
            Guild? guild = _context.Guilds.FirstOrDefault(x => x.Id == arg.Id);
            if (guild == null)
            {
                return;
            }

            _context.Guilds.Remove(guild);
            await _context.SaveChangesAsync();
        }

        private async Task JoinedGuild(SocketGuild arg)
        {
            Guild? guild = _context.Guilds.FirstOrDefault(x => x.Id == arg.Id);
            if (guild != null)
            {
                return;
            }

            guild = new(arg.Id);
            _context.Guilds.Add(guild);

            await _context.SaveChangesAsync();

            _ = Task.Run(async () =>
            {
                await arg.DownloadUsersAsync();

                foreach (IUser guser in arg.Users)
                {
                    User user = await AddOrGetUser(guser.Id);
                    GuildUser guildUser = new(user, guild, DateTime.UtcNow);
                    user.Guilds.Add(guildUser);
                    guild.Users.Add(guildUser);
                    _context.GuildUsers.Add(guildUser);
                }

                await _context.SaveChangesAsync();
            });
        }

        public ScheduledTask MigrationTask => scheduledTask;

        public async Task<User> AddOrGetUser(ulong id)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                user = new(id);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
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
            Guild? guild = _context.Guilds.FirstOrDefault(x => x.Id == dguild.Id);

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
            _context.Guilds.Update(guild);

            // commit changes to the database.
            Task task = _context.SaveChangesAsync();
            await task;
            if (task.IsFaulted)
            {
                await log.LogAsync(new(LogSeverity.Warning, "Activity Update Task", "Updating the database failed", task.Exception));
            }
        }

        public ModerationContext Context => _context;

        public async Task DoTasks()
        {
            await UpdateGuilds();

            await UpdateUsers();

            await UpdateRoles();

            await UpdateInactivities();
        }

        public async Task DoTasksFor(Guild guild)
        {
            await UpdateUsers(guild);

            await UpdateRoles(guild);

            await UpdateInactivities(guild);
        }

        #region Update methods

        public async Task UpdateUsers()
        {
            List<Guild> removeQueue = new();
            foreach (Guild guild in _context.Guilds)
            {
                Task task = UpdateUsers(guild);
                await task;
                if (task.IsFaulted)
                {
                    removeQueue.Add(guild);
                    await log.LogAsync(new LogMessage(LogSeverity.Warning, nameof(UpdateRoles), task.Exception?.ToString()));
                }
            }

            _context.Guilds.RemoveRange(removeQueue);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoles()
        {
            List<Guild> removeQueue = new();
            foreach (Guild guild in _context.Guilds)
            {
                Task task = UpdateRoles(guild);
                await task;
                if (task.IsFaulted)
                {
                    removeQueue.Add(guild);
                    await log.LogAsync(new LogMessage(LogSeverity.Warning, nameof(UpdateRoles), task.Exception?.ToString()));
                }
            }

            _context.Guilds.RemoveRange(removeQueue);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInactivities()
        {
            List<Guild> removeQueue = new();
            foreach (Guild guild in _context.Guilds)
            {
                Task task = UpdateInactivities(guild);
                await task;
                if (task.IsFaulted)
                {
                    removeQueue.Add(guild);
                    await log.LogAsync(new LogMessage(LogSeverity.Warning, nameof(UpdateInactivities), task.Exception?.ToString()));
                }
            }

            _context.Guilds.RemoveRange(removeQueue);
            await _context.SaveChangesAsync();
        }

        #endregion Update methods

        public async Task UpdateGuilds()
        {
            Queue<Guild> removeQueue = new();

            HashSet<ulong> guilds = _context.Guilds.Select(x => x.Id).ToHashSet();

            foreach (IGuild dguild in _client.Guilds)
            {
                if (guilds.Contains(dguild.Id))
                    continue;
                Guild guild = new(dguild.Id);
                await _context.Guilds.AddAsync(guild);
            }

            foreach (Guild guild in _context.Guilds)
            {
                if (_client.GetGuild(guild.Id) == null)
                {
                    removeQueue.Enqueue(guild);
                }
            }

            _context.Guilds.RemoveRange(removeQueue);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUsers(Guild guild)
        {
            SocketGuild dguild = _client.GetGuild(guild.Id);

            await dguild.DownloadUsersAsync();

            HashSet<ulong> gusers = dguild.Users.Where(x => !x.IsBot).Select(x => x.Id).ToHashSet();
            HashSet<ulong> dusers = guild.Users.Select(x => x.User.Id).ToHashSet();

            // searches for new users.
            foreach (IUser guser in dguild.Users)
            {
                // ignores already existing users and bots.
                if (dusers.Contains(guser.Id))
                    continue;

                // creates a new user or uses an existing one from the database.
                User? user = _context.Users.FirstOrDefault(u => u.Id == guser.Id);
                if (user == null)
                {
                    user = new(guser.Id);
                    await _context.Users.AddAsync(user);
                }

                // creates a new join entity.
                GuildUser guildUser = new(user, guild, DateTime.UtcNow);

                // adds the user to the guild.
                guild.Users.Add(guildUser);
                user.Guilds.Add(guildUser);

                // adds the user to the user table in the database.
                await _context.GuildUsers.AddAsync(guildUser);
                _context.Guilds.Update(guild);
            }

            // search for not existing users.
            List<GuildUser> removeQueue = new();
            foreach (GuildUser user in guild.Users)
            {
                if (!gusers.Contains(user.User.Id))
                    removeQueue.Add(user);
            }

            guild.Users.RemoveAll(removeQueue);

            _context.Guilds.Update(guild);
            _context.GuildUsers.RemoveAll(removeQueue);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoles(Guild guild)
        {
            SocketGuild dguild = _client.GetGuild(guild.Id);
            HashSet<ulong> groles = guild.Roles.Select(x => x.Id).ToHashSet();
            HashSet<ulong> droles = dguild.Roles.Select(x => x.Id).ToHashSet();

            foreach (IRole drole in guild.Roles)
            {
                if (!groles.Contains(drole.Id))
                {
                    Role role = new(drole.Id);
                    guild.Roles.Add(role);
                    _context.Roles.Add(role);
                }
            }

            List<Role> removeQueue = new();

            // goes through all ignores roles
            foreach (Role role in guild.Roles)
            {
                // if the role does not exist in the guild then we enqueue it into the queue.
                if (!droles.Contains(role.Id))
                    removeQueue.Add(role);
            }

            // removes all enqueued roles that are not longer exists.
            guild.Roles.RemoveAll(removeQueue);
            _context.Roles.RemoveAll(removeQueue);

            _context.Guilds.Update(guild);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInactivities(Guild guild)
        {
            if (!guild.ActivityKicking)
                return;

            SocketGuild? dguild = _client.GetGuild(guild.Id);

            // searches for inactive users.
            foreach (GuildUser user in guild.Users)
            {
                IGuildUser guser = dguild.GetUser(user.User.Id);

                // checks if the user is a bot and if the user is getting tracked else we just skip the user.
                if (guser.IsBot || guser.RoleIds.Any(x => guild.IgnoredRoles.Any(y => y.Id == x)))
                {
                    continue;
                }

                // if the user has no activity after the given time and the user has no warn, the user will be warned.
                if (user.LastActivity + guild.DurationWarn < DateTime.UtcNow && !user.HasActivityWarn)
                {
                    await guser.SendMessageAsync("ye wiww be wemoved fwom ffe gwain of sawt sewvew fow inactivity in 5 days; to awoid ffis pwease type smff in chat");
                    user.HasActivityWarn = true;
                    user.ActivityWarn = DateTime.UtcNow;
                    _context.GuildUsers.Update(user);
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
            await _context.SaveChangesAsync();
        }
    }
}