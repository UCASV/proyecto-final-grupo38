using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoFinal.VaccinationDB
{
    public partial class SideEffect
    {
        public int Id { get; set; }
        public string SideEffect1 { get; set; }
        public int IdVaccine { get; set; }

        public virtual Vaccine IdVaccineNavigation { get; set; }
    }
}
