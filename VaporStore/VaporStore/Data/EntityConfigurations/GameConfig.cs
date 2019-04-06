using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaporStore.Data.Models;

namespace VaporStore.Data.EntityConfigurations
{
    public class GameConfig : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
                .Property(g => g.Name)
                .IsRequired();

            builder.HasMany(g => g.Purchases)
                .WithOne(p => p.Game)
                .HasForeignKey(p => p.GameId);

            builder
                .HasMany(g => g.GameTags)
                .WithOne(gt => gt.Game)
                .HasForeignKey(gt => gt.GameId);
        }
    }

    //• Id – integer, Primary Key
    //• Name – text (required)
    //• Price – decimal (non-negative, minimum value: 0) (required)
    //• ReleaseDate – Date (required)
    //• DeveloperId – integer, foreign key (required)
    //• Developer – the game’s developer (required)
    //• GenreId – integer, foreign key (required)
    //• Genre – the game’s genre (required)
    //• Purchases - collection of type Purchase
    //• GameTags - collection of type GameTag. Each game must have at least one tag.
}