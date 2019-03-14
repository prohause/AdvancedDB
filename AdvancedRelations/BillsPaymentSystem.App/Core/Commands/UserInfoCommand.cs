using BillsPaymentSystem.App.Core.Commands.Contracts;
using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models.Enums;
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

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(userId), "User not found!");
            }

            var paymentMethods = _context.PaymentMethods.Where(p => p.UserId == user.UserId).Include(b => b.BankAccount).Include(c => c.CreditCard).ToList();
            var bankAccounts = paymentMethods.Where(p => p.PaymentType == PaymentType.BankAccount).ToList();
            var creditCardAccounts = paymentMethods.Where(p => p.PaymentType == PaymentType.CreditCard).ToList();

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