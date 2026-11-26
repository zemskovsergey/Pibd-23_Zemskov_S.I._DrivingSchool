using System;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;

namespace DrivingSchool.Forms
{
    public partial class CategoriesForm : Form
    {
        private readonly ICategoryRepository _repo;
        private DataGridView grid;

        public CategoriesForm(ICategoryRepository repo)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            this.Text = "Справочник: Категории прав";
            this.Width = 750;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            var btnAdd = new Button { Text = "Добавить категорию", Dock = DockStyle.Top, Height = 35 };
            btnAdd.Click += (s, e) => {
                using (var f = new AddCategoryForm())
                {
                    if (f.ShowDialog() == DialogResult.OK && f.CreatedCategory != null)
                    {
                        _repo.CreateCategory(f.CreatedCategory);
                        LoadData();
                    }
                }
            };

            grid = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false };
            this.Controls.Add(grid);
            this.Controls.Add(btnAdd);
            LoadData();
        }

        private void LoadData() => grid.DataSource = _repo.ReadCategories();
    }
}