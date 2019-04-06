using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaporStore.Data.Models;

namespace VaporStore.Data.EntityConfigurations
{
    public class TagConfig : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder
                .Property(t => t.Name)
                .IsRequired();

            builder
                .HasMany(t => t.GameTags)
                .WithOne(gt => gt.Tag)
                .HasForeignKey(gt => gt.TagId);
        }
    }
}