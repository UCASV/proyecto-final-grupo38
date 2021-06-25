using System;
using System.Drawing;
using System.Windows.Forms;
using ProyectoFinal.VaccinationDB;

namespace ProyectoFinal
{
    public partial class FrmCreateEmployee : Form
    {
        public FrmCreateEmployee()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text.Trim() =="" || txtName.Text.Trim() == "" || TxtAddres.Text.Trim() =="")
            {
                MessageBox.Show("fill in all the fields.", "Employee Registration",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var stringChars = new char[5];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars);

                int type = cmbType.Text.Trim() switch
                {
                    "Vacunador" => 2,
                    "Supervisor" => 3,
                    "Organizador" => 4,
                    "Conductor" => 5,
                    _ => 1
                };

                var username = txtUsername.Text.Trim();
                var pass = txtPassword.Text.Trim();
                if (txtUsername.Text.Trim() == "")
                {
                    username = null;
                    pass = null;
                }

                using (var db = new VaccinationDBContext())
                {
                    var Employee = new Employee
                    {
                        Identifier = finalString,
                        Username = username,
                        Password = pass,
                        IdType = type,
                        FullName = txtName.Text.Trim(),
                        Address = TxtAddres.Text.Trim(),
                        Email = txtEmail.Text.Trim()

                    };
                    // Save into DB
                    db.Add(Employee);
                    db.SaveChanges();

                    MessageBox.Show("Employee registered succesfully."+ Environment.NewLine
                        + "Your Identifier is: "+ finalString, "Employee Registration",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                }
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.Text.Trim() != "Gestor")
            {
                txtUsername.Text = "";
                txtPassword.Text = "";
                lblWarning.Visible = true;
                lblWarning.ForeColor = Color.White;
            }else
                lblWarning.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}