using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Services;
using ProyectoFinal.VaccinationDB;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;


namespace ProyectoFinal
{
    public partial class FrmPrincipal : Form
    {
        private System.Windows.Forms.TextBox txtIdCubicle;
        private System.Windows.Forms.TextBox txtDose;
        private System.Windows.Forms.TextBox txtIdPatient;
        private Label label6;
        private System.Windows.Forms.Button button3;
        private Label label9;
        private TextBox textBox7;
        private Label label10;
        private TextBox txtPrintIdAppointment;

        public FrmPrincipal()
        {
            InitializeComponent();
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
                MessageBox.Show("Do not leave blank spaces please.", "Appointment creation", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else if (dateTimePicker1.Value < DateTime.Today)
            {
                MessageBox.Show("Incorrect date, check again.", "Appointment creation", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else if (now < start || now > end)
            {
                MessageBox.Show("Incorrect time, check again.", "Appointment creation", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else if (!CheckCabin(Int32.Parse(txtIdCubicle.Text)) || !validations.ValidateNumbersOnly(txtIdCubicle.Text))
            {
                MessageBox.Show("Cabin ID is incorrect.", "Appointment creation", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else if (!validations.ValidateNumbersOnly(txtIdPatient.Text))
            {
                MessageBox.Show("DUI format is incorrect.", "Appointment creation", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                var dui = txtIdPatient.Text.Insert(8, "-");
                if (CheckCitizen(dui))
                {
                    using (var db = new VaccinationDBContext())
                    {
                        var Appointment = new Appointment
                        {
                            Date = dateTimePicker1.Value,
                            Hour = dateTimePicker3.Value.TimeOfDay,
                            IdCabin = Int32.Parse(txtIdCubicle.Text.Trim()),
                            DuiCitizen = dui,
                            Dose = txtDose.Text.Trim()
                        };
                        // Save into DB
                        db.Add(Appointment);
                        db.SaveChanges();
                    }

                    MessageBox.Show("Appointment created.", "Appointment creation", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("DUI might be wrong.", "Appointment creation", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var validations = new Validations();
            
            if (!validations.ValidateEmail(txtCreateEmailCubicle.Text))
            {
                MessageBox.Show("Wrong data, please check again.", "Cabin creation", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
                var db = new VaccinationDBContext();
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
        
        private bool CheckCitizen(string dui)
        {
            var db = new VaccinationDBContext(/*userResult[0] */);
            List<Citizen> citizens = db.Citizens
                .Include(c => c.Vaccines).ToList();
            
            List<Citizen> citizenResult = citizens
                .Where(u => u.Dui == dui)
                .ToList();


            return citizenResult.Count > 0;

        }
        
        
        private bool CheckCabin(int id)
        {
            var db = new VaccinationDBContext();
            List<Cabin> cabins = db.Cabins
                .Include(c => c.Registries).ToList();
            
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
                .Include(e => e.Registries).ToList();
            
            List<Employee> employeeResult = employees
                .Where(e => e.Identifier == id)
                .ToList();

            if (employeeResult.Count > 0) // If user exists
            {
                return false;
            }
            else // If user doesn't exist 
            {
                MessageBox.Show("There are no users matching this DUI!", "El Salvador's Vaccination - Add Medical Condition",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return true;
            }
        }

        private void btnCreateAppointment_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            tabControl1.Visible = true;
        }

        private void BtnCreateCubicle_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            tabControl1.Visible = true;
        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Cerrando la aplicacion cuando se cierre el form
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrmCreaterUser window = new FrmCreaterUser();
            window.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var db2 = new VaccinationDBContext();
            // cargando la lista de las citas si el usuario 
            var appointments = db2.Appointments
                .Include(i => i.DuiCitizenNavigation).ToList();
            
            Dgv.DataSource = null;
            Dgv.DataSource = appointments;
            
            Dgv.Columns["DuiCitizenNavigation"].Visible = false;
            Dgv.Columns["IdCabinNavigation"].Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Dgv.RowCount == 0)
            { 
                MessageBox.Show("There no data!");
            }
            else
            {    //Guardando ruta del Pdf
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*";
                if (save.ShowDialog() == DialogResult.OK)
                {
                    string filename = save.FileName; 
                    Document doc = new Document(PageSize.A3, 9, 9, 9, 9);
                    Chunk encab = new Chunk("REPORT", FontFactory.GetFont("COURIER", 18));
                    try
                    {
                        FileStream file = new FileStream(filename, FileMode.OpenOrCreate);
                        PdfWriter writer = PdfWriter.GetInstance(doc, file);
                        writer.ViewerPreferences = PdfWriter.PageModeUseThumbs;
                        writer.ViewerPreferences = PdfWriter.PageLayoutOneColumn;
                        doc.Open();

                        doc.Add(new Paragraph(encab));
                        GenerDoc(doc);

                        Process.Start(filename);
                        doc.Close();
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        public void GenerDoc(Document document)
       {
           //se crea un objeto PdfTable con el numero de columnas del dataGridView 
           PdfPTable datatable = new PdfPTable(Dgv.ColumnCount);

           //asignamos algunas propiedades para el diseño del pdf 
           datatable.DefaultCell.Padding = 1;
           float[] headerwidths = GetTamañoColumnas(Dgv);

           datatable.SetWidths(headerwidths);
           datatable.WidthPercentage = 100;
           datatable.DefaultCell.BorderWidth = 1;

           //definir color
           datatable.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE;

          //definimos color de bordes
           datatable.DefaultCell.BorderColor = iTextSharp.text.BaseColor.BLACK;

         // definimos la fuente
           iTextSharp.text.Font fuente = new   iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA);

           Phrase objP = new Phrase("A", fuente);

           datatable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

           //Se genera el emcabezado del pdf
           for (int i = 0; i < Dgv.ColumnCount; i++)
           {

               objP = new Phrase(Dgv.Columns[i].HeaderText, fuente);
               datatable.HorizontalAlignment = Element.ALIGN_CENTER;

               datatable.AddCell(objP);

           }
           datatable.HeaderRows = 2;

           datatable.DefaultCell.BorderWidth = 1;

           //Se genere al pdf
           for (int i = 0; i < Dgv.RowCount; i++)
           {
               for (int j = 0; j < Dgv.ColumnCount; j++)
               {
                   objP = new Phrase(Dgv[j, i].Value.ToString(), fuente);
                   datatable.AddCell(objP);
               }
               datatable.CompleteRow();
           }
           document.Add(datatable);
       }

       //Función que obtiene los tamaños de las columnas del datagridview
       public float[] GetTamañoColumnas(DataGridView dg)
       {
           //Tomamos el numero de columnas
           float[] values = new float[dg.ColumnCount];
           for (int i = 0; i < dg.ColumnCount; i++)
           {
               //Tomamos el ancho de cada columna
               values[i] = (float)dg.Columns[i].Width;
           }
           return values;
       }
    }
}