using Depot.Enitities;
using Discord;
using Microsoft.EntityFrameworkCore;

namespace Depot.Contexts
{
    public class ModerationContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<GuildUser> GuildUsers { get; set; }

        public ModerationContext()
        {
            Database.Migrate();
        }

        public bool AddOrGetGuild(IGuild dguild, out Guild guild)
        {
            return AddOrGetGuild(dguild.Id, out guild);
        }

        public bool AddOrGetGuild(ulong guildid, out Guild guild)
        {
            Guild? _guild = GetGuild(guildid);
            if (_guild == null)
            {
                guild = new(guildid);
                Guilds.Add(guild);
                return true;
            }

            guild = _guild;
            return false;
        }

        public User AddOrGetUser(IUser duser)
        {
            return AddOrGetUser(duser.Id);
        }

        public User AddOrGetUser(ulong userid)
        {
            User? user = GetUser(userid);
            if (user == null)
            {
                user = new(userid);
                Users.Add(user);
            }

            return user;
        }

        public bool AddOrGetRole(IRole drole, out Role role)
        {
            return AddOrGetRole(drole.Id, out role);
        }

        public bool AddOrGetRole(ulong roleid, out Role role)
        {
            Role? _role = GetRole(roleid);
            if (_role == null)
            {
                role = new(roleid);
                Roles.Add(role);
                return true;
            }

            role = _role;
            return false;
        }

        public Guild? GetGuild(IGuild guild)
        {
            return GetGuild(guild.Id);
        }

        public Guild? GetGuild(ulong id)
        {
            return Guilds.FirstOrDefault(x => x.Id == id);
        }

        public User? GetUser(IUser user)
        {
            return GetUser(user.Id);
        }

        public User? GetUser(ulong id)
        {
            return Users.FirstOrDefault(x => x.Id == id);
        }

        public Role? GetRole(IRole role)
        {
            return GetRole(role.Id);
        }

        public Role? GetRole(ulong id)
        {
            return Roles.FirstOrDefault(x => x.Id == id);
        }

        public Role? GetRoleOf(IGuild guild, IRole role)
        {
            return GetRoleOf(guild.Id, role.Id);
        }

        public Role? GetRoleOf(IGuild guild, ulong roleId)
        {
            return GetRoleOf(guild.Id, roleId);
        }

        public Role? GetRoleOf(ulong guildId, ulong roleId)
        {
            return GetGuild(guildId)?.Roles.FirstOrDefault(x => x.Id == roleId);
        }

        public GuildUser? GetGuildUser(IGuildUser guildUser)
        {
            return GetGuildUser(guildUser.GuildId, guildUser.Id);
        }

        public GuildUser? GetGuildUser(IGuild guild, IUser user)
        {
            return GetGuildUser(guild.Id, user.Id);
        }

        public GuildUser? GetGuildUser(ulong guildId, ulong userId)
        {
            return GetGuild(guildId)?.Users.FirstOrDefault(x => x.UserId == userId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite("Data Source=database.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<GuildUser>()
                .HasKey(gu => new { gu.UserId, gu.GuildId });

            modelBuilder
                .Entity<GuildUser>()
                .HasOne(u => u.Guild)
                .WithMany(u => u.Users)
                .HasForeignKey(gu => gu.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<GuildUser>()
                .HasOne(u => u.User)
                .WithMany(u => u.Guilds)
                .HasForeignKey(gu => gu.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}