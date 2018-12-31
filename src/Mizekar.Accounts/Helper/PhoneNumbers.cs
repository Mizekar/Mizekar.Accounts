using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mizekar.Core.Extensions;

namespace Mizekar.Accounts.Helper
{
    public static class PhoneNumbers
    {
        /// <summary>
        /// Removes all non numeric characters from a string
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private static string RemoveNonNumeric(string phone)
        {
            return Regex.Replace(phone, @"[^0-9]+", "");
        }

        public static bool ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
            var cleanNumber = phoneNumber.ToEnglishNumbers();
            cleanNumber = RemoveNonNumeric(cleanNumber);
            return cleanNumber.Length == 12; // 989108802440
        }

        public static string NormalizePhoneNumber(string phoneNumber)
        {
            var cleanNumber = phoneNumber.ToEnglishNumbers();
            cleanNumber = RemoveNonNumeric(cleanNumber);
            return cleanNumber;
        }
    }
}
