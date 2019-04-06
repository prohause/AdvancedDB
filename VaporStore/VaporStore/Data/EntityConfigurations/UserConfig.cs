using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaporStore.Data.Models;

namespace VaporStore.Data.EntityConfigurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(u => u.Username)
                .IsRequired();

            builder
                .Property(u => u.FullName)
                .IsRequired();

            builder
                .Property(u => u.Email)
                .IsRequired();

            builder
                .HasMany(u => u.Cards)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
        }
    }

    //User
    //• Id – integer, Primary Key
    //• Username – text with length [3, 20] (required)
    //• FullName – text, which has two words, consisting of Latin letters. Both start with an upper letter and are separated by a single space (ex. "John Smith") (required)
    //• Email – text (required)
    //• Age – integer in the range [3, 103] (required)
    //• Cards – collection of type Card
}