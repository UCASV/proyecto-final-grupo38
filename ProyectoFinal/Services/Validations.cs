using System;
using System.Linq;

namespace ProyectoFinal.Services
{
    public class Validations
    {
        public bool ValidateNumbersOnly(string text)
        {
            return ValidateEmpty(text) && text.All(char.IsDigit);
        }
        
        public bool ValidateLettersOnly(string text)
        {
            return ValidateEmpty(text) && text.All(char.IsLetter);
        }

        public bool ValidateEmail(string email)
        {
            return ValidateEmpty(email) && email.Contains('@') && email.EndsWith(".com") || email.EndsWith(".es") || email.EndsWith(".sv");
        }

        public bool ValidateEmpty(string text)
        {
            return text.Length > 0;
        }
    }
}