using System.Text.RegularExpressions;

namespace EventRegistrationSystem.Services
{
    class ValidationService
    {
        // Checks if text is empty
        public static bool IsNotEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        // Checks if email contains @
        public static bool IsValidEmail(string email)
        {
            return email.Contains("@");
        }

        // Checks if contact number is exactly 11 digits
        public static bool IsValidContact(string contact)
        {
            return Regex.IsMatch(contact, @"^\d{11}$");
        }
    }
}