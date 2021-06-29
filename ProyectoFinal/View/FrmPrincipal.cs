using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
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
        }

        private void button4_Click(object sender, EventArgs e)
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
            }
        }
    }
}