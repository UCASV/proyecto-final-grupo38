using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoFinal.VaccinationDB
{
    public partial class Citizen
    {
        public Citizen()
        {
            Appointments = new HashSet<Appointment>();
            CitizenxEmployees = new HashSet<CitizenxEmployee>();
            Diseases = new HashSet<Disease>();
            Vaccines = new HashSet<Vaccine>();
        }

        public string Dui { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int IdIdentifer { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<CitizenxEmployee> CitizenxEmployees { get; set; }
        public virtual ICollection<Disease> Diseases { get; set; }
        public virtual ICollection<Vaccine> Vaccines { get; set; }
    }
}
