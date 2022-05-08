namespace Depot.Enitities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Warning
    {
        public Warning()
        {
        }

        public Warning(ulong user, string message, DateTime timestap)
        {
            UserId = user;
            Message = message;
            Timestamp = timestap;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public virtual ulong Id { get; set; }

        public virtual ulong UserId { get; set; }

        public virtual string Message { get; set; } = string.Empty;

        public virtual DateTime Timestamp { get; set; }
    }
}