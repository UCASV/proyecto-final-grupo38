using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoFinal.VaccinationDB
{
    public partial class Cabin
    {
        public Cabin()
        {
            Appointments = new HashSet<Appointment>();
            Registries = new HashSet<Registry>();
        }

        public int Id { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string IdentifierEmployee { get; set; }

        public virtual Employee IdentifierEmployeeNavigation { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Registry> Registries { get; set; }
    }
}
