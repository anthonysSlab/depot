namespace Depot.Enitities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class IgnoredRole
    {
#nullable disable

        [Obsolete("EF ctor do not use")]
        public IgnoredRole(int id, ulong roleId)
        {
            Id = id;
            RoleId = roleId;
        }

#nullable enable

        public IgnoredRole(Role role)
        {
            Role = role;
            RoleId = role.Id;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public virtual int Id { get; set; }

        public virtual ulong RoleId { get; set; }

        public virtual Role Role { get; set; }

        public static implicit operator IgnoredRole(Role role)
        {
            return new IgnoredRole(role);
        }
    }
}