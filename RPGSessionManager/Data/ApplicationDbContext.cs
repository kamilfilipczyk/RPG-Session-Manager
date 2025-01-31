using Microsoft.EntityFrameworkCore;
using RPGSessionManager.Models;

namespace RPGSessionManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
