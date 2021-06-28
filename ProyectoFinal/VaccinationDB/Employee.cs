using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoFinal.VaccinationDB
{
    public partial class Employee
    {
        public Employee()
        {
            Cabins = new HashSet<Cabin>();
            CitizenxEmployees = new HashSet<CitizenxEmployee>();
            Registries = new HashSet<Registry>();
            Vaccines = new HashSet<Vaccine>();
        }

        public string Identifier { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int IdType { get; set; }

        public virtual EmployeeType IdTypeNavigation { get; set; }
        public virtual ICollection<Cabin> Cabins { get; set; }
        public virtual ICollection<CitizenxEmployee> CitizenxEmployees { get; set; }
        public virtual ICollection<Registry> Registries { get; set; }
        public virtual ICollection<Vaccine> Vaccines { get; set; }
    }
}
