using Instagraph.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instagraph.Data.EntityConfigurations
{
    public class PictureConfig : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder
                .HasMany(u => u.Users)
                .WithOne(p => p.ProfilePicture)
                .HasForeignKey(p => p.ProfilePictureId);

            builder
                .HasMany(p => p.Posts)
                .WithOne(p => p.Picture)
                .HasForeignKey(p => p.PictureId);

            builder
                .Property(p => p.Path)
                .IsRequired();
        }
    }
}