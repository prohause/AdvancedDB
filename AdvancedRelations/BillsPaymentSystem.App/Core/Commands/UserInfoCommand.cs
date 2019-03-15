using BillsPaymentSystem.App.Core.Commands.Contracts;
using BillsPaymentSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;

namespace BillsPaymentSystem.App.Core.Commands
{
    public class UserInfoCommand : ICommand
    {
        private readonly BillsPaymentSystemContext _context;

        public UserInfoCommand(BillsPaymentSystemContext context)
        {
            _context = context;
        }

        public string Execute(string[] args)
        {
            var userId = int.Parse(args[0]);

            var user = _context.Users.Include(p => p.PaymentMethods).ThenInclude(b => b.BankAccount).Include(c => c.PaymentMethods).ThenInclude(c => c.CreditCard).FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(userId), "User not found!");
            }

            var bankAccounts = user.PaymentMethods.Where(p => p.BankAccountId != null).OrderBy(b => b.BankAccountId).ToList();
            var creditCardAccounts = user.PaymentMethods.Where(p => p.CreditCardId != null).OrderBy(c => c.CreditCardId).ToList();

            var sb = new StringBuilder();
            sb.AppendLine($"User: {user.FirstName} {user.LastName}");
            sb.AppendLine("Bank Accounts:");
            foreach (var bankAccount in bankAccounts)
            {
                sb.AppendLine($"-- ID: {bankAccount.BankAccountId}\n--- Balance: {bankAccount.BankAccount.Balance}\n--- Bank: {bankAccount.BankAccount.BankName}\n--- SWIFT: {bankAccount.BankAccount.SwiftCode}");
            }

            sb.AppendLine("Credit Cards:");
            foreach (var creditCardAccount in creditCardAccounts)
            {
                sb.AppendLine($"-- ID: {creditCardAccount.CreditCardId}\n--- Limit: {creditCardAccount.CreditCard.Limit}\n--- Limit Left:: {creditCardAccount.CreditCard.LimitLeft}\n--- Expiration Date: {creditCardAccount.CreditCard.ExpirationDate}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}