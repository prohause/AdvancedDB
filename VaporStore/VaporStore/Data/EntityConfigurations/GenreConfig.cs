using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaporStore.Data.Models;

namespace VaporStore.Data.EntityConfigurations
{
    public class GenreConfig : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder
                .Property(g => g.Name)
                .IsRequired();

            builder
                .HasMany(g => g.Games)
                .WithOne(g => g.Genre)
                .HasForeignKey(g => g.GenreId);
        }
    }
}