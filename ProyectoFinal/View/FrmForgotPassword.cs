using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using ProyectoFinal.VaccinationDB;

namespace ProyectoFinal
{
    public partial class FrmForgotPassword : Form
    {
        public FrmForgotPassword()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (TxtUsernameFP.Text.Trim() == "" || TextIdentifierFP.Text.Trim() == "") //validación por si esta vacío el textbox
            {
                MessageBox.Show("enter valid values.",
                    "El Salvador's Vaccination Forgot Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var db = new VaccinationDBContext();
                List<Employee> users = db.Employees
                    .Include(u => u.Vaccines).ToList();

                var userid = TxtUsernameFP.Text;
                var identifier = TextIdentifierFP.Text;

                List<Employee> userResult = users
                    .Where(u => u.Username == userid && u.Identifier == identifier)
                    .ToList();
                if (userResult.Count == 0) //verificamos si existe el usuario para poder mostrarle su pregunta
                {
                    MessageBox.Show("enter a valid user.",
                        "El Salvador's Vaccination Forgot Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    TxtPasswordFP.Text = userResult[0].Password;
                }
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            MessageBox.Show("returning to login ", "El Salvador's Vaccination Forgot Password",
                MessageBoxButtons.OK, MessageBoxIcon.Information); 
            this.Hide();
        }
        
    }
}