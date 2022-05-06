namespace Depot.Enitities
{
    public class GuildUser
    {
        public GuildUser()
        {
        }

        public GuildUser(User user, Guild guild, DateTime lastActivity)
        {
            User = user;
            Guild = guild;
            LastActivity = lastActivity;
            HasActivityWarn = false;
            ActivityWarn = default;
        }

        public virtual User User { get; set; }

        public virtual ulong UserId { get; set; }

        public virtual Guild Guild { get; set; }

        public virtual ulong GuildId { get; set; }

        public virtual DateTime LastActivity { get; set; }

        public virtual bool HasActivityWarn { get; set; }

        public virtual DateTime ActivityWarn { get; set; }

        public virtual List<Warning> Warnings { get; set; } = new();
    }
}