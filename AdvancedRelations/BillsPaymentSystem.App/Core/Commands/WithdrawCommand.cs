using BillsPaymentSystem.App.Core.Commands.Contracts;
using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillsPaymentSystem.App.Core.Commands
{
    public class WithdrawCommand : ICommand
    {
        private readonly BillsPaymentSystemContext _context;

        public WithdrawCommand(BillsPaymentSystemContext context)
        {
            _context = context;
        }

        public string Execute(string[] args)
        {
            var userId = int.Parse(args[0]);
            var amount = decimal.Parse(args[1]);
            var result = "";

            var user = _context.Users.Include(p => p.PaymentMethods).ThenInclude(b => b.BankAccount).Include(c => c.PaymentMethods).ThenInclude(c => c.CreditCard).FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(userId), "User not found!");
            }

            var payMethods = user.PaymentMethods.OrderByDescending(p => p.PaymentType).ThenBy(p => p.BankAccountId)
                .ThenBy(p => p.CreditCardId).ToList();

            if (!MoneyAvailable(payMethods, amount))
            {
                result = "Not Enough Money";
            }

            return result;
        }

        private static bool MoneyAvailable(List<PaymentMethod> payMethods, decimal amount)
        {
            var result = payMethods.Any(b => b.BankAccountId != null && b.BankAccount.Balance >= amount || b.CreditCardId != null && b.CreditCard.LimitLeft >= amount);

            return result;
        }
    }
}