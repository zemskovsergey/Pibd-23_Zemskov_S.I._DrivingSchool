using System;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;

namespace DrivingSchool.Forms
{
    public partial class GroupsForm : Form
    {
        private readonly IGroupRepository _repo;
        private DataGridView grid;

        public GroupsForm(IGroupRepository repo)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            this.Text = "Справочник: Группы";
            this.Width = 750;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            var btnAdd = new Button { Text = "Добавить группу", Dock = DockStyle.Top, Height = 35 };
            btnAdd.Click += (s, e) => {
                using (var f = new AddGroupForm())
                {
                    if (f.ShowDialog() == DialogResult.OK && f.CreatedGroup != null)
                    {
                        _repo.CreateGroup(f.CreatedGroup);
                        LoadData();
                    }
                }
            };

            grid = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false };
            this.Controls.Add(grid);
            this.Controls.Add(btnAdd);
            LoadData();
        }

        private void LoadData() => grid.DataSource = _repo.ReadGroups();
    }
}