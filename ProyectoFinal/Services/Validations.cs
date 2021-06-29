using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.VaccinationDB;

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
            return ValidateEmpty(text) && text.All(char.IsLetter) || text.Contains(" ");
        }

        public bool ValidateEmail(string email)
        {
            return ValidateEmpty(email) && email.Contains('@') && email.EndsWith(".com") || email.EndsWith(".es") ||
                   email.EndsWith(".sv");
        }

        public bool ValidateEmpty(string text)
        {
            return text.Length > 0;
        }

        public bool ValidateCitizen(Citizen citizen)
        {


            if (citizen.IdIdentifer != 1)
            {
                if (checkDisease(citizen.Dui))
                {

                }

                return true;
            }

            return false;
        }

        private bool checkDisease(string dui)
        {
            var db = new VaccinationDBContext( /*userResult[0] */);
            List<Disease> diseases = db.Diseases
                .Include(d => d.DuiCitizen).ToList();

            List<Disease> diseaseResult = diseases
                .Where(d => d.DuiCitizen == dui)
                .ToList();

            if (diseaseResult.Count > 0) // If user exists
            {
                return true;
            }
            else // If user doesn't exist 
            {
                MessageBox.Show("There are no users matching this DUI!",
                    "El Salvador's Vaccination - Add Medical Condition",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;

            }
        }
    }
}