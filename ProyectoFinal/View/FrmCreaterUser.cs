using System;
using System.Windows.Forms;
using ProyectoFinal.VaccinationDB;

namespace ProyectoFinal
{
    public partial class FrmCreaterUser : Form
    {
        public FrmCreaterUser()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var db = new VaccinationDBContext();
            Disease disease = new Disease
            {
                Disease1 = "Gripe",
                DuiCitizen = "05958448-3"
            };

            db.Add(disease);
            db.SaveChanges();
        }
    }
}