using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BillsPaymentSystem.Data.EntityConfigurations
{
    public class PaymentMethodConfig : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder
                .HasOne(p => p.CreditCard)
                .WithOne(c => c.PaymentMethod)
                .HasForeignKey<CreditCard>(p => p.CreditCardId);

            builder
                .HasOne(p => p.BankAccount)
                .WithOne(b => b.PaymentMethod)
                .HasForeignKey<BankAccount>(p => p.BankAccountId);
        }
    }
}