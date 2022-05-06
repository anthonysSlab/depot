using Depot.Enitities;
using Microsoft.EntityFrameworkCore;

namespace Depot.Contexts
{
    public class ModerationContext : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<GuildUser> GuildUsers { get; set; }

        public ModerationContext()
        {
            Database.EnsureCreated();
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlite("Data Source=database.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<GuildUser>()
                .HasKey(gu => new { gu.UserId, gu.GuildId });

            modelBuilder
                .Entity<GuildUser>()
                .HasOne(u => u.Guild)
                .WithMany(u => u.Users)
                .HasForeignKey(gu => gu.GuildId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<GuildUser>()
                .HasOne(u => u.User)
                .WithMany(u => u.Guilds)
                .HasForeignKey(gu => gu.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}