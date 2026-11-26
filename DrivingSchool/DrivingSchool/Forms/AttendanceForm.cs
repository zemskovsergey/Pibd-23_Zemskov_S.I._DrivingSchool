using System;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;

namespace DrivingSchool.Forms
{
    public partial class AttendanceForm : Form
    {
        private readonly IAttendanceRepository _repo;
        private DataGridView grid;

        public AttendanceForm(IAttendanceRepository repo)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            this.Text = "Журнал: Посещаемость";
            this.Width = 750;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            var btnAdd = new Button { Text = "Отметить посещаемость", Dock = DockStyle.Top, Height = 35 };
            btnAdd.Click += (s, e) => {
                using (var f = new AddAttendanceForm())
                {
                    if (f.ShowDialog() == DialogResult.OK && f.CreatedAttendance != null)
                    {
                        _repo.CreateAttendance(f.CreatedAttendance);
                        LoadData();
                    }
                }
            };

            grid = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false };
            this.Controls.Add(grid);
            this.Controls.Add(btnAdd);
            LoadData();
        }

        private void LoadData() => grid.DataSource = _repo.ReadAttendances();
    }
}