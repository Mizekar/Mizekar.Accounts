using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Mizekar.Accounts.Helper;

namespace Mizekar.Accounts.Validation
{
    public class PhoneOrEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phoneOrEmail = (string)value;
            if (phoneOrEmail.Contains("@"))
            {
                if (new EmailAddressAttribute().IsValid(phoneOrEmail) == false)
                {
                    return new ValidationResult("Email is not valid");
                }
            }
            else
            {
                if (PhoneNumbers.ValidatePhoneNumber(phoneOrEmail) == false)
                {
                    return new ValidationResult("PhoneNumber is not valid. valid phone is like 989*********");
                }
            }

            return ValidationResult.Success;
        }


    }
}
