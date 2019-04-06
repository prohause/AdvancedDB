using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaporStore.Data.Models;

namespace VaporStore.Data.EntityConfigurations
{
    public class CardConfig : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder
                .Property(c => c.Number)
                .IsRequired();

            builder
                .Property(c => c.Cvc)
                .IsRequired();

            builder
                .Property(c => c.Type)
                .IsRequired();

            builder
                .HasMany(c => c.Purchases)
                .WithOne(p => p.Card)
                .HasForeignKey(p => p.CardId);
        }

        //• Id – integer, Primary Key
        //• Number – text, which consists of 4 pairs of 4 digits, separated by spaces (ex. “1234 5678 9012 3456”) (required)
        //• Cvc – text, which consists of 3 digits (ex. “123”) (required)
        //• Type – enumeration of type CardType, with possible values (“Debit”, “Credit”) (required)
        //• UserId – integer, foreign key (required)
        //• User – the card’s user (required)
        //• Purchases – collection of type Purchase
    }
}