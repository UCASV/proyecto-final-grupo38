using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Services;
using ProyectoFinal.VaccinationDB;

namespace ProyectoFinal
{
    public partial class FrmPrincipal : Form
    {
        private System.Windows.Forms.TextBox txtIdCubicle;
        private System.Windows.Forms.TextBox txtDose;
        private System.Windows.Forms.TextBox txtIdPatient;
        private Label label6;
        private Button button3;
        private Label label9;
        private TextBox textBox7;
        private Label label10;
        private TextBox txtPrintIdAppointment;

        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void newPatientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void appointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            tabControl1.Visible = true;
        }


        private void createUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCreaterUser window = new FrmCreaterUser();
            window.ShowDialog();
        }

        private void createAppointmentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            tabControl1.Visible = true;
        }

        private void createAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            tabControl1.Visible = true;
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size(0, 1);

            var db2 = new VaccinationDBContext();
            // cargando la lista de las citas si el usuario no es nuevo
            var appointments = db2.Appointments
                .Include(i => i.DuiCitizenNavigation).ToList();
            
            Dgv.DataSource = null;
            Dgv.DataSource = appointments;
            
            Dgv.Columns["DuiCitizenNavigation"].Visible = false;
            Dgv.Columns["IdCabinNavigation"].Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var validations = new Validations();
            // Hour validation
            TimeSpan start = new TimeSpan(06, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            TimeSpan now = dateTimePicker3.Value.TimeOfDay;
            
            if (!validations.ValidateEmpty(txtIdPatient.Text) || !validations.ValidateEmpty(txtIdCubicle.Text) 
                                                              || !validations.ValidateEmpty(txtDose.Text))
            {
                MessageBox.Show("Do not leave blank spaces please.", "Appointment creation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (dateTimePicker1.Value < DateTime.Today)
            {
                MessageBox.Show("Incorrect date, check again.", "Appointment creation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (now < start || now > end)
            {
                MessageBox.Show("Incorrect time, check again.", "Appointment creation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (CheckCitizen(txtIdPatient.Text) || CheckCabin(Int32.Parse(txtIdCubicle.Text)))
            {
                MessageBox.Show("Patient DUI or cabin ID are incorrect.", "Appointment creation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                using (var db = new VaccinationDBContext())
                {
                    var Appointment = new Appointment
                    {
                        Date = dateTimePicker1.Value,
                        Hour = dateTimePicker3.Value.TimeOfDay,
                        IdCabin = Int32.Parse(txtIdCubicle.Text.Trim()),
                        DuiCitizen= txtIdPatient.Text.Trim(),
                        Dose = txtDose.Text.Trim()
                    };
                    // Save into DB
                    db.Add(Appointment);
                    db.SaveChanges();
                }
                MessageBox.Show("Appointment created.", "Appointment creation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var validations = new Validations();
            
            if (!validations.ValidateEmail(txtCreateEmailCubicle.Text))
            {
                MessageBox.Show("Mail format is incorrect.", "Cabin creation", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else if (!validations.ValidateEmpty(txtCreateAddresCubicle.Text))
            {
                MessageBox.Show("Do not leave blank spaces please.", "Cabin creation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!validations.ValidateNumbersOnly(txtCreateNumerPhoneCubicle.Text))
            {
                MessageBox.Show("Wrong phone format.", "Cabin creation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (CheckEmployee(txtCreateIdEmployee.Text))
            {
                MessageBox.Show("Employee not found.", "Cabin creation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                using (var db = new VaccinationDBContext())
                {
                    var Cabin = new Cabin
                    {
                        Address = txtCreateAddresCubicle.Text.Trim(),
                        Phone = txtCreateNumerPhoneCubicle.Text.Trim(),
                        IdentifierEmployee = txtCreateIdEmployee.Text.Trim(),
                        Email= txtCreateEmailCubicle.Text.Trim()
                    };
                    // Save into DB
                    db.Add(Cabin);
                    db.SaveChanges();
                    MessageBox.Show("Cubicle created.", "Cubicle creation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
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
                MessageBox.Show("There are no users matching this DUI!", "El Salvador's Vaccination - Add Medical Condition",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
            }
        }
        
        private bool CheckCabin(int id)
        {
            var db = new VaccinationDBContext();
            List<Cabin> cabins = db.Cabins
                .Include(c => c.Id).ToList();
            
            List<Cabin> cabinResult = cabins
                .Where(c => c.Id == id)
                .ToList();

            if (cabinResult.Count > 0) // If cabin exists
            {
                return true;
            }
            else // If empty 
            {
                MessageBox.Show("There are no cabins matching this ID!", "Create appointment",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
            }
        }
        
        private bool CheckEmployee(string id)
        {
            var db = new VaccinationDBContext();
            List<Employee> employees = db.Employees
                .Include(e => e.Identifier).ToList();
            
            List<Employee> employeeResult = employees
                .Where(e => e.Identifier == id)
                .ToList();

            if (employeeResult.Count > 0) // If user exists
            {
                return true;
            }
            else // If user doesn't exist 
            {
                MessageBox.Show("There are no users matching this DUI!", "El Salvador's Vaccination - Add Medical Condition",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
            }
        }
    }
}