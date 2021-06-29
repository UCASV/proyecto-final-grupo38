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
            bool validForAppointment = citizen.IdIdentifer != 1 || checkDisease(citizen.Dui);


            return validForAppointment;
        }

        private bool checkDisease(string dui)
        {
            var db = new VaccinationDBContext( /*userResult[0] */);
            List<Disease> diseases = db.Diseases
                .Include(d => d.DuiCitizen).ToList();

            List<Disease> diseaseResult = diseases
                .Where(d => d.DuiCitizen == dui)
                .ToList();

            return diseaseResult.Count > 0;
        }
    }
}