using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RPGSessionManager.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace RPGSessionManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
        new IdentityRole
        {
            Id = "admin",
            Name = "admin",
            NormalizedName = "ADMIN"
        },
        new IdentityRole
        {
            Id = "player",
            Name = "player",
            NormalizedName = "PLAYER"
        }
    );

            // Seed users
            var hasher = new PasswordHasher<IdentityUser>();

            var adminUser = new IdentityUser
            {
                Id = "1",
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin123");

            var playerUser = new IdentityUser
            {
                Id = "2",
                UserName = "player@example.com",
                NormalizedUserName = "PLAYER@EXAMPLE.COM",
                Email = "player@example.com",
                NormalizedEmail = "PLAYER@EXAMPLE.COM",
                EmailConfirmed = true
            };
            playerUser.PasswordHash = hasher.HashPassword(playerUser, "player123");

            modelBuilder.Entity<IdentityUser>().HasData(adminUser, playerUser);

            // Seed user roles
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "1",
                    RoleId = "admin"
                },
                new IdentityUserRole<string>
                {
                    UserId = "2",
                    RoleId = "player"
                }
            );

            modelBuilder.Entity<Team>().HasData(
            new Team { Id = 1, Name = "Team 1", About = "A very cool team", HomeCity = "Kraków" }
        );

            modelBuilder.Entity<Player>().HasData(
                new Player { Id = 1, Name = "Krzysztof" },
                new Player { Id = 2, Name = "Adam" }
            );

            modelBuilder.Entity<Campaign>().HasData(
                new Campaign { Id = 1, Title = "W mroku", About = "Mega fajna kampania", HasEnded = false }
            );

            modelBuilder.Entity<Session>().HasData(
                new Session { Id = 1, Title = "Lecimy na grubo", About = "Lecimy na grubo albo wcale", Date = DateOnly.Parse("2025-01-30"), CampaignId = 1, TeamId = 1 }
            );

            modelBuilder.Entity<Character>().HasData(
                new Character { Id = 1, FirstName = "Character 1", LastName = "One", Profession = "Warrior", PlayerId = 1, TeamId = 1 },
                new Character { Id = 2, FirstName = "Character 2", LastName = "Two", Profession = "Mage", PlayerId = 2, TeamId = 1 }
            );
        }
    }
}
