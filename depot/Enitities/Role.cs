namespace Depot.Enitities
{
    public class Role
    {
#nullable disable

        [Obsolete("EF ctor do not use")]
        public Role(ulong id)
        {
            Id = id;
        }

#nullable enable

        public Role(ulong id, Guild guild)
        {
            Id = id;
            Guild = guild;
            Users = new();
        }

        public virtual ulong Id { get; set; }

        public virtual Guild Guild { get; set; }

        public virtual List<GuildUser> Users { get; set; }
    }
}