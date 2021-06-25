using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoFinal.VaccinationDB
{
    public partial class CitizenxEmployee
    {
        public string DuiCitizen { get; set; }
        public string IdentifierEmployee { get; set; }

        public virtual Citizen DuiCitizenNavigation { get; set; }
        public virtual Employee IdentifierEmployeeNavigation { get; set; }
    }
}
