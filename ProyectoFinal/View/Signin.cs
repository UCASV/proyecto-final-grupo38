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
            FrmCreaterUser window = new FrmCreaterUser();
            window.ShowDialog();
        }

        private void BtnForgoPassword_Click(object sender, EventArgs e)
        {
            FrmForgotPassword window = new FrmForgotPassword();
            window.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() == "" &&
                txtPassword.Text.Trim() == "") //validación por si esta vacío el textbox
            {
                //Mensaje de Error
                MessageBox.Show("Los campos estan vacios :(", "Vacunación El Salvador",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                //conectamos con base de datos para verificar si existe el usuario
                // y tiene la misma contraseña
                var db = new VaccinationDBContext(/*userResult[0] */);
                List<Employee> users = db.Employees
                    .Include(u => u.Vaccines).ToList();
                    
                string userid = txtUsername.Text;
                string password = txtPassword.Text;

                // Usuario que tiene el mismo carnet
                List<Employee> userResult = users
                    .Where(u => u.Username == userid && u.Password == password)
                    .ToList();

                if (userResult.Count > 0) //si existe
                {
                    MessageBox.Show("Bienvenido al portal de Vacunación!", "Vacunación El Salvador",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FrmPrincipal window = new FrmPrincipal();
                    window.Show();
                    this.Hide();
                }
                else //si no existe
                {
                    MessageBox.Show("Usuario no existe!", "Vacunación El Salvador",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}