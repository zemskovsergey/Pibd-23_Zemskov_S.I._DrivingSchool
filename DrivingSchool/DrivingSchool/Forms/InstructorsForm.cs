using System;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;

namespace DrivingSchool.Forms
{
    public partial class InstructorsForm : Form
    {
        private readonly IInstructorRepository _repo;
        private DataGridView grid;

        public InstructorsForm(IInstructorRepository repo)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            this.Text = "Справочник: Преподаватели";
            this.Width = 750;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            var btnAdd = new Button { Text = "Добавить преподавателя", Dock = DockStyle.Top, Height = 35 };
            btnAdd.Click += (s, e) => {
                using (var f = new AddInstructorForm())
                {
                    if (f.ShowDialog() == DialogResult.OK && f.CreatedInstructor != null)
                    {
                        _repo.CreateInstructor(f.CreatedInstructor);
                        LoadData();
                    }
                }
            };

            grid = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false };
            this.Controls.Add(grid);
            this.Controls.Add(btnAdd);
            LoadData();
        }

        private void LoadData() => grid.DataSource = _repo.ReadInstructors();
    }
}