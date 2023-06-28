using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentProg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = StudentsSource;
            if (BusinessLayer.Students.UpdateStudents() != -1)
            {
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                StudentsSource.DataSource = StudentProgData.Students.GetStudents();
                StudentsSource.Sort = "StudId";
                dataGridView1.DataSource = StudentsSource;

                dataGridView1.Columns["StudId"].HeaderText = "Student ID";
                dataGridView1.Columns["StudId"].DisplayIndex = 0;
                dataGridView1.Columns["Name"].DisplayIndex = 1;
                dataGridView1.Columns["YearEnrolment"].DisplayIndex = 2;
                dataGridView1.Columns["ProgId"].DisplayIndex = 3;
            }
        }

        private void programsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = ProgramsSource;
            if (BusinessLayer.Programs.UpdatePrograms() != -1)
            {

                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
                dataGridView1.RowHeadersVisible = true;
                dataGridView1.Dock = DockStyle.Fill;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                ProgramsSource.DataSource = StudentProgData.Programs.GetPrograms();
                ProgramsSource.Sort = "ProgId";
                dataGridView1.DataSource = ProgramsSource;
            }
        }

        private void ProgramsSource_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Programs.UpdatePrograms();
        }

        private void StudentsSource_CurrentChanged(object sender, EventArgs e)
        {
            BusinessLayer.Students.UpdateStudents();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Erreur de données: " + e.Exception.Message);
            dataGridView1.CancelEdit();
        }

        internal static void badYear()
        {
            // Reponse probleme 3
            MessageBox.Show("L'année d'inscription doit être entre 2017 et 2023"); // Voir ligne 27 et la suite de fichier BLL.cs
        }
    }


}

