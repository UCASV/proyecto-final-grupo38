using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoFinal.VaccinationDB
{
    public partial class Vaccine
    {
        public Vaccine()
        {
            SideEffects = new HashSet<SideEffect>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Hour { get; set; }
        public string Dose { get; set; }
        public string DuiCitizen { get; set; }
        public string IdentifierEmployee { get; set; }

        public virtual Citizen DuiCitizenNavigation { get; set; }
        public virtual Employee IdentifierEmployeeNavigation { get; set; }
        public virtual ICollection<SideEffect> SideEffects { get; set; }
    }
}
