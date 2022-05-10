namespace Depot.Enitities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Warning
    {
#nullable disable

        public Warning()
        {
        }

#nullable enable

        public Warning(GuildUser user, string message, DateTime timestap)
        {
            User = user;
            Message = message;
            Timestamp = timestap;
        }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public virtual ulong Id { get; set; }

        public virtual GuildUser User { get; set; }

        public virtual string Message { get; set; } = string.Empty;

        public virtual DateTime Timestamp { get; set; }
    }
}