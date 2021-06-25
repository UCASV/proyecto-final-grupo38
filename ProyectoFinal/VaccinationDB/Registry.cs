using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoFinal.VaccinationDB
{
    public partial class Registry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Hour { get; set; }
        public string Phone { get; set; }
        public string IdentifierEmployee { get; set; }
        public int IdCabin { get; set; }

        public virtual Cabin IdCabinNavigation { get; set; }
        public virtual Employee IdentifierEmployeeNavigation { get; set; }
    }
}
