namespace Depot.Enitities
{
    public class Role
    {
        public Role()
        {
        }

        public Role(ulong id)
        {
            Id = id;
        }

        public virtual ulong Id { get; set; }
    }
}