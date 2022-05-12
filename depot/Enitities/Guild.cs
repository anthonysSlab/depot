namespace Depot.Enitities
{
    using Discord;

    public class Guild
    {
#nullable disable

        [Obsolete("EF ctor do not use")]
        public Guild(ulong id, bool activityKicking, TimeSpan durationWarn, TimeSpan durationKick)
        {
            Id = id;
            ActivityKicking = activityKicking;
            DurationWarn = durationWarn;
            DurationKick = durationKick;
        }

#nullable enable

        public Guild(ulong id)
        {
            Id = id;
            Users = new();
            Roles = new();
            IgnoredRoles = new();
            Warnings = new();
        }

        public virtual ulong Id { get; set; }

        public virtual bool ActivityKicking { get; set; }

        public virtual TimeSpan DurationWarn { get; set; }

        public virtual TimeSpan DurationKick { get; set; }

        public virtual List<GuildUser> Users { get; set; }

        public virtual List<Role> Roles { get; set; }

        public virtual List<IgnoredRole> IgnoredRoles { get; set; }

        public virtual List<Warning> Warnings { get; set; }

        public GuildUser? GetGuildUser(IUser user)
        {
            return GetGuildUser(user.Id);
        }

        public GuildUser? GetGuildUser(User user)
        {
            return GetGuildUser(user.Id);
        }

        public GuildUser? GetGuildUser(ulong user)
        {
            return Users.FirstOrDefault(u => u.UserId == user);
        }

        public Role? GetRole(IRole role)
        {
            return GetRole(role.Id);
        }

        public Role? GetRole(ulong role)
        {
            return Roles.FirstOrDefault(u => u.Id == role);
        }
    }
}