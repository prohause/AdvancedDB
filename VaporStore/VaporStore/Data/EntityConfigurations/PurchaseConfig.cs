using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VaporStore.Data.Models;

namespace VaporStore.Data.EntityConfigurations
{
    public class PurchaseConfig : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder
                .Property(p => p.Type)
                .IsRequired();
        }
    }

    //• Id – integer, Primary Key
    //• Type – enumeration of type PurchaseType, with possible values (“Retail”, “Digital”) (required)
    //• ProductKey – text, which consists of 3 pairs of 4 uppercase Latin letters and digits, separated by dashes (ex. “ABCD-EFGH-1J3L”) (required)
    //• Date – Date (required)
    //• CardId – integer, foreign key (required)
    //• Card – the purchase’s card (required)
    //• GameId – integer, foreign key (required)
    //• Game – the purchase’s game (required)
}