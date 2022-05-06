namespace Depot.Enitities
{
    public class Guild
    {
        public Guild()
        {
        }

        public Guild(ulong id)
        {
            Id = id;
        }

        public virtual ulong Id { get; set; }

        public virtual bool ActivityKicking { get; set; }

        public virtual TimeSpan DurationWarn { get; set; }

        public virtual TimeSpan DurationKick { get; set; }

        public virtual List<GuildUser> Users { get; set; } = new();

        public virtual List<Role> IgnoredRoles { get; set; } = new();

        public virtual List<Warning> Warnings { get; set; } = new();
    }
}