namespace Depot.Enitities
{
    public class User
    {
        public User(ulong id)
        {
            Id = id;
        }

        public virtual ulong Id { get; set; }

        public virtual List<GuildUser> Guilds { get; set; } = new();

        public virtual List<Warning> Warnings { get; set; } = new();
    }
}