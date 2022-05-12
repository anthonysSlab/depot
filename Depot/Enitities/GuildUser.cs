namespace Depot.Enitities
{
    public class GuildUser
    {
#nullable disable

        [Obsolete("EF ctor do not use")]
        public GuildUser(ulong userId, ulong guildId, DateTime lastActivity, bool hasActivityWarn, DateTime activityWarn)
        {
            User = null;
            UserId = userId;
            Guild = null;
            GuildId = guildId;
            Roles = null;
            LastActivity = lastActivity;
            HasActivityWarn = hasActivityWarn;
            ActivityWarn = activityWarn;
        }

#nullable enable

        public GuildUser(User user, Guild guild, DateTime lastActivity)
        {
            User = user;
            Guild = guild;
            Roles = new();
            LastActivity = lastActivity;
            HasActivityWarn = false;
            ActivityWarn = default;
        }

        public virtual User User { get; set; }

        public virtual ulong UserId { get; set; }

        public virtual Guild Guild { get; set; }

        public virtual ulong GuildId { get; set; }

        public virtual List<Role> Roles { get; set; }

        public virtual DateTime LastActivity { get; set; }

        public virtual bool HasActivityWarn { get; set; }

        public virtual DateTime ActivityWarn { get; set; }
    }
}