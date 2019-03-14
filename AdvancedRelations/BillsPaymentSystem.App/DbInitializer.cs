using BillsPaymentSystem.Data;
using BillsPaymentSystem.Models;
using BillsPaymentSystem.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillsPaymentSystem.App
{
    public class DbInitializer
    {
        public static void Seed(BillsPaymentSystemContext context)
        {
            SeedUser(context);
            SeedCreditCards(context);
            SeedBankAccounts(context);
            SeedPaymentMethod(context);
        }

        private static void SeedPaymentMethod(BillsPaymentSystemContext context)
        {
            var paymentMethods = new List<PaymentMethod>();

            for (int i = 0; i < 3; i++)
            {
                var paymentMethod = new PaymentMethod
                {
                    UserId = new Random().Next(1, 5),
                    PaymentType = (PaymentType)new Random().Next(0, 2),
                };

                if (i % 3 == 0)
                {
                    paymentMethod.CreditCardId = new Random().Next(1, 5);
                    paymentMethod.BankAccountId = new Random().Next(1, 5);
                }
                else if (i % 2 == 0)
                {
                    paymentMethod.CreditCardId = new Random().Next(1, 5);
                }
                else
                {
                    paymentMethod.BankAccountId = new Random().Next(1, 5);
                }

                if (IsValid(paymentMethod))
                {
                    paymentMethods.Add(paymentMethod);
                }
            }

            context.PaymentMethods.AddRange(paymentMethods);
            context.SaveChanges();
        }

        private static void SeedBankAccounts(BillsPaymentSystemContext context)
        {
            var bankAccounts = new List<BankAccount>();

            for (var i = 0; i < 8; i++)
            {
                var bankAccount = new BankAccount
                {
                    Balance = new Random().Next(-25000, 25000),
                    BankName = "Bank" + i,
                    SwiftCode = "SWIFT" + i + i
                };

                if (IsValid(bankAccount))
                {
                    bankAccounts.Add(bankAccount);
                }
            }
            context.BankAccounts.AddRange(bankAccounts);
            context.SaveChanges();
        }

        private static void SeedCreditCards(BillsPaymentSystemContext context)
        {
            var creditCards = new List<CreditCard>();

            for (var i = 0; i < 8; i++)
            {
                var creditCard = new CreditCard
                {
                    Limit = new Random().Next(-250000, 250000),
                    MoneyOwed = new Random().Next(-250000, 250000),
                    ExpirationDate = DateTime.Now.AddDays(new Random().Next(-200, 200))
                };

                if (IsValid(creditCard))
                {
                    creditCards.Add(creditCard);
                }
            }
            context.CreditCards.AddRange(creditCards);
            context.SaveChanges();
        }

        private static void SeedUser(BillsPaymentSystemContext context)
        {
            string[] firstNames = { "Ivan", "Dragan", "Petkan", "Stoyan", null, "" };
            string[] lastNames = { "Ivanov", "Draganov", "Petkanov", "Stoyanov", null, "" };
            string[] emails = { "ivan@abv.bg", "dragan@mail.bg", "petkan@gmail.com", "stoyan@live.com", null, "" };
            string[] passwords = { "1234", "5678", "1346", "9876", null, "" };

            var users = new List<User>();

            for (int i = 0; i < firstNames.Length; i++)
            {
                var user = new User
                {
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    Email = emails[i],
                    Password = passwords[i]
                };

                if (IsValid(user))
                {
                    users.Add(user);
                }
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResults = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(entity, validationContext, validationResults, true);

            return isValid;
        }
    }
}