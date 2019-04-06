using VaporStore.Data.EntityConfigurations;
using VaporStore.Data.Models;

namespace VaporStore.Data
{
    using Microsoft.EntityFrameworkCore;

    public class VaporStoreDbContext : DbContext
    {
        public VaporStoreDbContext()
        {
        }

        public VaporStoreDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Developer> Developers { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<GameTag> GameTags { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.ApplyConfiguration(new CardConfig());
            model.ApplyConfiguration(new DeveloperConfig());
            model.ApplyConfiguration(new GameConfig());
            model.ApplyConfiguration(new GameTagConfig());
            model.ApplyConfiguration(new GenreConfig());
            model.ApplyConfiguration(new PurchaseConfig());
            model.ApplyConfiguration(new TagConfig());
            model.ApplyConfiguration(new UserConfig());
        }
    }
}