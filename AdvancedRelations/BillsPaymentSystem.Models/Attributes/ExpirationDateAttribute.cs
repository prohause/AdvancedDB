using System;
using System.ComponentModel.DataAnnotations;

namespace BillsPaymentSystem.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExpirationDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentDateTime = DateTime.Now;
            var targetDateTime = (DateTime)value;

            return currentDateTime >= targetDateTime ? new ValidationResult("Credit card has expired") : ValidationResult.Success;
        }
    }
}