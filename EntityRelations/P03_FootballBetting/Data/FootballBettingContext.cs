using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Team>(entity =>
                {
                    entity
                        .HasKey(e => e.TeamId);

                    entity
                        .HasOne(e => e.PrimaryKitColor)
                        .WithMany(c => c.PrimaryKitTeams);

                    entity
                        .HasOne(e => e.SecondaryKitColor)
                        .WithMany(s => s.SecondaryKitTeams);

                    entity
                        .HasOne(e => e.Town)
                        .WithMany(t => t.Teams);

                    entity
                        .HasMany(e => e.HomeGames)
                        .WithOne(g => g.HomeTeam);

                    entity
                        .HasMany(e => e.AwayGames)
                        .WithOne(g => g.AwayTeam);
                });

            modelBuilder
                .Entity<Color>(entity =>
                {
                    entity
                        .HasKey(e => e.ColorId);
                });

            modelBuilder
                .Entity<Town>(entity =>
                {
                    entity
                        .HasKey(e => e.TownId);

                    entity
                        .HasOne(e => e.Country)
                        .WithMany(c => c.Towns);
                });

            modelBuilder
                .Entity<Country>(entity =>
                {
                    entity
                        .HasKey(e => e.CountryId);
                });

            modelBuilder
                .Entity<Player>(entity =>
                {
                    entity
                        .HasKey(e => e.PlayerId);

                    entity
                        .HasOne(e => e.Team)
                        .WithMany(t => t.Players);

                    entity
                        .HasOne(e => e.Position)
                        .WithMany(p => p.Players);
                });

            modelBuilder
                .Entity<Position>(entity =>
                {
                    entity
                        .HasKey(e => e.PositionId);
                });

            modelBuilder
                .Entity<PlayerStatistic>(entity =>
                {
                    entity
                        .HasKey(e => new
                        {
                            e.PlayerId,
                            e.GameId
                        });

                    entity
                        .HasOne(e => e.Player)
                        .WithMany(p => p.PlayerStatistics)
                        .HasForeignKey(e => e.PlayerId);

                    entity
                        .HasOne(e => e.Game)
                        .WithMany(g => g.PlayerStatistics)
                        .HasForeignKey(e => e.GameId);
                });

            modelBuilder
                .Entity<Game>(entity =>
                {
                    entity
                        .HasKey(e => e.GameId);

                    entity
                        .HasMany(e => e.Bets)
                        .WithOne(b => b.Game);
                });

            modelBuilder
                .Entity<Bet>(entity =>
                {
                    entity
                        .HasKey(e => e.BetId);

                    entity
                        .Property(e => e.Prediction)
                        .IsRequired();
                });

            modelBuilder
                .Entity<User>(entity =>
                {
                    entity
                        .HasKey(e => e.UserId);

                    entity
                        .HasMany(e => e.Bets)
                        .WithOne(b => b.User);
                });
        }
    }
}