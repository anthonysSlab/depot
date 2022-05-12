namespace Depot.Enitities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Warning
    {
#nullable disable

        [Obsolete("EF ctor do not use")]
        public Warning(ulong id, ulong userId, ulong guildId, string message, DateTime timestamp)
        {
            Id = id;
            UserId = userId;
            GuildId = guildId;
            Message = message;
            Timestamp = timestamp;
        }

#nullable enable

        public Warning(User user, Guild guild, string message, DateTime timestap)
        {
            User = user;
            UserId = user.Id;
            Guild = guild;
            GuildId = guild.Id;
            Message = message;
            Timestamp = timestap;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public virtual ulong Id { get; set; }

        public virtual ulong UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ulong GuildId { get; set; }

        public virtual Guild Guild { get; set; }

        public virtual string Message { get; set; } = string.Empty;

        public virtual DateTime Timestamp { get; set; }
    }
}