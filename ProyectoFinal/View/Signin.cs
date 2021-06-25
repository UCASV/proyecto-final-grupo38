using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using ProyectoFinal.Services;
using ProyectoFinal.VaccinationDB;

namespace ProyectoFinal
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnCreateUser_Click(object sender, EventArgs e)
        {
            FrmCreateEmployee window = new FrmCreateEmployee();
            window.ShowDialog();
        }

        private void BtnForgoPassword_Click(object sender, EventArgs e)
        {
            FrmForgotPassword window = new FrmForgotPassword();
            window.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "" && txtPassword.Text.Trim() == "") // Check if empty
            {
                // Error message
                MessageBox.Show("Blank spaces are not allowed :(", "El Salvador's Vaccination",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Check on db if user exists
                // If exists check password
                var db = new VaccinationDBContext(/*userResult[0] */);
                List<Employee> users = db.Employees
                    .Include(u => u.Vaccines).ToList();
                    
                string userid = txtUsername.Text;
                string password = txtPassword.Text;

                // User that has same id
                List<Employee> userResult = users
                    .Where(u => u.Username == userid && u.Password == password)
                    .ToList();

                if (userResult.Count > 0) // If user exists
                {
                    MessageBox.Show("Welcome to Vaccination System!", "El Salvador's Vaccination",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FrmPrincipal window = new FrmPrincipal();
                    window.Show();
                    this.Hide();
                }
                else // If user doesn't exist 
                {
                    MessageBox.Show("User or password incorrect!", "El Salvador's Vaccination",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}