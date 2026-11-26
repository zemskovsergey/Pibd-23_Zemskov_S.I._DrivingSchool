using System;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;

namespace DrivingSchool.Forms
{
    public partial class StudentsForm : Form
    {
        private readonly IStudentRepository _studentRepository;
        private DataGridView dataGridViewStudents;
        private Button btnAdd;
        private Button btnDelete;

        public StudentsForm(IStudentRepository studentRepository)
        {
            InitializeComponent();
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));

            SetupForm();
            LoadData();
        }

        private void SetupForm()
        {
            this.Text = "Справочник: Студенты";
            this.Width = 800;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterParent;

            var topPanel = new Panel { Dock = DockStyle.Top, Height = 45 };

            btnAdd = new Button { Text = "Добавить студента", Left = 10, Top = 8, Width = 150, Height = 30 };
            btnAdd.Click += BtnAdd_Click;

            btnDelete = new Button { Text = "Удалить выбранного", Left = 170, Top = 8, Width = 160, Height = 30 };
            btnDelete.Click += BtnDelete_Click;

            topPanel.Controls.Add(btnAdd);
            topPanel.Controls.Add(btnDelete);

            dataGridViewStudents = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            this.Controls.Add(dataGridViewStudents);
            this.Controls.Add(topPanel);
        }

        private void LoadData()
        {
            dataGridViewStudents.DataSource = null;
            dataGridViewStudents.DataSource = _studentRepository.ReadStudents();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddStudentForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK && addForm.CreatedStudent != null)
                {
                    _studentRepository.CreateStudent(addForm.CreatedStudent);
                    LoadData();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewStudents.CurrentRow?.DataBoundItem is Student selectedStudent)
            {
                var confirm = MessageBox.Show($"Удалить студента {selectedStudent.FullName}?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    _studentRepository.DeleteStudent(selectedStudent.StudentId);
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}