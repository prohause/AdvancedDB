using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaporStore.Data.Models;

namespace VaporStore.Data.EntityConfigurations
{
    public class GameTagConfig : IEntityTypeConfiguration<GameTag>
    {
        public void Configure(EntityTypeBuilder<GameTag> builder)
        {
            builder.HasKey(gt => new
            {
                gt.GameId,
                gt.TagId
            });
        }
    }
}