using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
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
            var citizen = new Citizen
            {
                Dui = txtDui.Text,
                FullName = txtFullName.Text,
                Address = txtAddress.Text,
                Phone = txtPhone.Text,
                Email = txtEmail.Text,
                Identifier = txtId.Text
            };
            
            using (var db = new VaccinationDBContext())
            {
                var result = db.Citizens
                    .Where(c => c.Dui.Equals(citizen.Dui))
                    .ToList();

                if (result.Count() == 0)
                {
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