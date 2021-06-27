using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using ProyectoFinal.Services;
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
            var validations = new Validations();
            if (!validations.ValidateEmpty(TxtUsernameFP.Text) || !validations.ValidateEmpty(TextIdentifierFP.Text)) // Empty spaces validation
            {
                MessageBox.Show("Data is incorrect. Please enter valid values.",
                    "El Salvador's Vaccination - Forgot Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (userResult.Count == 0) // Check if user exists
                {
                    MessageBox.Show("The user does not exist. Please enter a valid user.",
                        "El Salvador's Vaccination - Forgot Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    TxtPasswordFP.Text = userResult[0].Password;
                }
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Returning to Login ", "El Salvador's Vaccination - Forgot Password",
                MessageBoxButtons.OK, MessageBoxIcon.Information); 
            this.Hide();
        }
        
    }
}