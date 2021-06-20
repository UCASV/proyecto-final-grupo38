using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using ProyectoFinal.Services;
using ProyectoFinal.VaccinationDB;

namespace ProyectoFinal
{
    public partial class FrmCreaterUser : Form
    {
        private System.Windows.Forms.RadioButton radNo;
        private TableLayoutPanel tlpCreateUser;

        public FrmCreaterUser()
        {
            InitializeComponent();
        }

        private void cbYes_CheckedChanged(object sender, EventArgs e)
        {
            txtDisease.ReadOnly = false;
            btnAddDisease.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var validations = new Validations();
            var dui = "";
            var fullName = "";
            var email = "";
            var address = "";
            var phone = "";
            var validData = false;

            if (validations.ValidateNumbersOnly(txtDui.Text) && validations.ValidateLettersOnly(txtFullName.Text) && 
                    validations.ValidateEmail(txtEmail.Text) && validations.ValidateNumbersOnly(txtPhone.Text) && validations.ValidateEmpty(txtAddress.Text))
            {
                dui = txtDui.Text.Insert(8, "-"); // Format DUI
                fullName = txtFullName.Text;
                address = txtAddress.Text;
                email = txtEmail.Text;
                phone = txtPhone.Text.Insert(4, "-"); // Format phone
                
                validData = true; // Allow registration
            }

            if (validData)
            {
                // Create new Citizen instance
                var citizen = new Citizen
                {
                    Dui = dui.Trim(),
                    FullName = fullName.Trim(),
                    Address = address.Trim(),
                    Phone = phone.Trim(),
                    Email = email.Trim(),
                    Identifier = txtId.Text.Trim()
                };
            
                using (var db = new VaccinationDBContext())
                {
                    // Check if there's another register with same DUI
                    var result = db.Citizens
                        .Where(c => c.Dui.Equals(citizen.Dui))
                        .ToList();

                    if (result.Count() == 0)
                    {
                        // Save into DB
                        db.Add(citizen);
                        db.SaveChanges();

                        MessageBox.Show("Citizen registered succesfully.", "Citizen Registration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Citizen has already been registered", "Citizen Registration", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                MessageBox.Show("Incorrect data, check again please.", "Citizen register", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        private void btnAddDisease_Click(object sender, EventArgs e)
        {
            if (txtDui.Text.Length < 10)
            {
                MessageBox.Show("You must introduce your DUI before adding a medical condition.", "Add Medical Condition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                var disease = new Disease
                {
                    Disease1 = txtDisease.Text,
                    DuiCitizen = txtDui.Text
                };

                using (var db = new VaccinationDBContext())
                {
                    var result = db.Diseases
                        .Where(d => d.Disease1.Equals(disease.Disease1))
                        .Where(d1 => d1.DuiCitizen.Equals(disease.DuiCitizen))
                        .ToList();

                    if (result.Count() == 0)
                    {
                        db.Add(disease);
                        db.SaveChanges();
                        
                        MessageBox.Show("Medical condition added succesfully.", "Add Medical Condition", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Medical condition has already been added.", "Add Medical Condition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                txtDisease.Clear();
            }
        }

        private void cbNo_CheckedChanged(object sender, EventArgs e)
        {
            txtDisease.ReadOnly = true;
            btnAddDisease.Enabled = false;
        }
    }
}