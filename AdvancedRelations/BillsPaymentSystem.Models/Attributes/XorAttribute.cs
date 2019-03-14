using System;
using System.ComponentModel.DataAnnotations;

namespace BillsPaymentSystem.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class XorAttribute : ValidationAttribute
    {
        private readonly string _targetProperty;

        public XorAttribute(string targetProperty)
        {
            _targetProperty = targetProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var targetPropertyValue = validationContext.ObjectType
                .GetProperty(_targetProperty)
                .GetValue(validationContext.ObjectInstance);

            if (value == null && targetPropertyValue == null || value != null && targetPropertyValue != null)
            {
                return new ValidationResult("The two properties must have opposite values!");
            }

            return ValidationResult.Success;
        }
    }
}