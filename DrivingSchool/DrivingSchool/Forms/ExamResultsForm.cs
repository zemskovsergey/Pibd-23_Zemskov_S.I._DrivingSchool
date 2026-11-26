using System;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;

namespace DrivingSchool.Forms
{
    public partial class ExamResultsForm : Form
    {
        private readonly IExamResultRepository _repo;
        private DataGridView grid;

        public ExamResultsForm(IExamResultRepository repo)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            this.Text = "Журнал: Результаты экзаменов";
            this.Width = 750;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            var btnAdd = new Button { Text = "Добавить результат", Dock = DockStyle.Top, Height = 35 };
            btnAdd.Click += (s, e) => {
                using (var f = new AddExamResultForm())
                {
                    if (f.ShowDialog() == DialogResult.OK && f.CreatedExamResult != null)
                    {
                        _repo.CreateExamResult(f.CreatedExamResult);
                        LoadData();
                    }
                }
            };

            grid = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false };
            this.Controls.Add(grid);
            this.Controls.Add(btnAdd);
            LoadData();
        }

        private void LoadData() => grid.DataSource = _repo.ReadExamResults();
    }
}