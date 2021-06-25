using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoFinal.VaccinationDB
{
    public partial class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Hour { get; set; }
        public string Dose { get; set; }
        public int IdCabin { get; set; }
        public string DuiCitizen { get; set; }

        public virtual Citizen DuiCitizenNavigation { get; set; }
        public virtual Cabin IdCabinNavigation { get; set; }
    }
}
