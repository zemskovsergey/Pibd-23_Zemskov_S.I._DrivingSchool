using System;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;

namespace DrivingSchool.Forms
{
    public partial class LessonsForm : Form
    {
        private readonly ILessonRepository _repo;
        private DataGridView grid;

        public LessonsForm(ILessonRepository repo)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            this.Text = "Журнал: Занятия";
            this.Width = 750;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            var btnAdd = new Button { Text = "Добавить занятие", Dock = DockStyle.Top, Height = 35 };
            btnAdd.Click += (s, e) => {
                using (var f = new AddLessonForm())
                {
                    if (f.ShowDialog() == DialogResult.OK && f.CreatedLesson != null)
                    {
                        _repo.CreateLesson(f.CreatedLesson);
                        LoadData();
                    }
                }
            };

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            this.Controls.Add(grid);
            this.Controls.Add(btnAdd);
            LoadData();
        }

        private void LoadData() => grid.DataSource = _repo.ReadLessons();
    }
}