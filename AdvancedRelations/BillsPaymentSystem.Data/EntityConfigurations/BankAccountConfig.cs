using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillsPaymentSystem.Data.EntityConfigurations
{
    public class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder
                .Property(b => b.BankName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(b => b.SwiftCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .IsRequired();
        }
    }
}