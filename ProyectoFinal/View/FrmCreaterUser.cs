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
    public partial class FrmCreaterUser : Form
    {
        private System.Windows.Forms.RadioButton radNo;
        private System.Windows.Forms.TableLayoutPanel tlpCreateUser;

        public FrmCreaterUser()
        {
            InitializeComponent();
        }

        private void cbYes_CheckedChanged(object sender, EventArgs e)
        {
            txtDisease.ReadOnly = false;
            btnAddDisease.Enabled = true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
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
                
                var identifier = CmbIdentifier.Text.Trim() switch
                {
                    "Education" => "ED948",
                    "Health" => "SA324",
                    "Civil national police" => "PNC21",
                    "government" => "G0001",
                    "Armed force" => "FA234",
                    "Journalism" => "P2834",
                    _ => "00000"
                };
                // Create new Citizen instance
                var citizen = new Citizen
                {
                    Dui = dui.Trim(),
                    FullName = fullName.Trim(),
                    Address = address.Trim(),
                    Phone = phone.Trim(),
                    Email = email.Trim(),
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
            var validations = new Validations();
            var dui = "";
            
            if (!validations.ValidateNumbersOnly(txtDui.Text))
            {
                MessageBox.Show("You must introduce your DUI before adding a medical condition.", "Add Medical Condition", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                dui = txtDui.Text.Insert(8, "-");

                if (CheckCitizen(dui))
                {
                    var disease = new Disease
                    {
                        Disease1 = txtDisease.Text,
                        DuiCitizen = dui 
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
        }

        private void cbNo_CheckedChanged(object sender, EventArgs e)
        {
            txtDisease.ReadOnly = true;
            btnAddDisease.Enabled = false;
        }

        private void radYes_CheckedChanged(object sender, EventArgs e)
        {
            txtDisease.ReadOnly = false;
            btnAddDisease.Enabled = true;
            
        }

        private bool CheckCitizen(string dui)
        {
            var db = new VaccinationDBContext(/*userResult[0] */);
            List<Citizen> citizens = db.Citizens
                .Include(c => c.Vaccines).ToList();
            
            List<Citizen> citizenResult = citizens
                .Where(u => u.Dui == dui)
                .ToList();

            if (citizenResult.Count > 0) // If user exists
            {
                return true;
            }
            else // If user doesn't exist 
            {
                MessageBox.Show("There are no users matching this DUI!", "El Salvador's Vaccination",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
            }
        }

        private void FrmCreaterUser_Load(object sender, EventArgs e)
        {
            ttpForm.SetToolTip(txtDui, "You must fill out this field in order to register. Only numbers allowed.");
            ttpForm.SetToolTip(txtAddress, "You must fill out this field in order to register.");
            ttpForm.SetToolTip(txtPhone, "You must fill out this field in order to register. Only numbers allowed. ");
            ttpForm.SetToolTip(txtFullName, "You must fill out this field in order to register.");
            ttpForm.SetToolTip(txtEmail, "This field is optional.");
            ttpForm.SetToolTip(CmbIdentifier, "This field is optional.");
            ttpForm.SetToolTip(txtDisease, "This field is optional.");
        }
    }
}