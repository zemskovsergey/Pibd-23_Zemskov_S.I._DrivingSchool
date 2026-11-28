using System;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DrivingSchool.Forms
{
    public partial class CategoriesForm : Form
    {
        private readonly ICategoryRepository _repo;
        private readonly DataGridView _grid;

        public CategoriesForm(ICategoryRepository repo)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));

            Text = "Справочник: Категории прав";
            Width = 750;
            Height = 450;
            StartPosition = FormStartPosition.CenterParent;

            var actions = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40 };
            var add = new Button { Text = "Добавить", AutoSize = true };
            var edit = new Button { Text = "Редактировать", AutoSize = true };
            var delete = new Button { Text = "Удалить", AutoSize = true };
            add.Click += (s, e) => RunAction(() => OpenEditor(null));
            edit.Click += (s, e) => RunAction(EditSelected);
            delete.Click += (s, e) => RunAction(DeleteSelected);
            actions.Controls.AddRange(new Control[] { add, edit, delete });

            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            _grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Название категории",
                DataPropertyName = nameof(Category.Name)
            });

            Controls.Add(_grid);
            Controls.Add(actions);
            Shown += (s, e) => RunAction(LoadData);
        }

        private void LoadData() => _grid.DataSource = _repo.ReadCategories().ToList();

        private Category GetSelectedCategory()
        {
            if (_grid.CurrentRow?.DataBoundItem is Category category)
                return category;

            MessageBox.Show(this, "Выберите категорию в списке.", "Категории",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        private void OpenEditor(Category existing)
        {
            using var dialog = new AddCategoryForm(existing);
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            if (existing == null)
                _repo.CreateCategory(dialog.CreatedCategory);
            else
                _repo.UpdateCategory(dialog.CreatedCategory);

            LoadData();
        }

        private void EditSelected()
        {
            var selected = GetSelectedCategory();
            if (selected == null)
                return;

            var current = _repo.ReadCategoryById(selected.CategoryId)
                ?? throw new InvalidOperationException("Категория больше не существует.");
            OpenEditor(current);
        }

        private void DeleteSelected()
        {
            var selected = GetSelectedCategory();
            if (selected == null || MessageBox.Show(this,
                $"Удалить категорию «{selected.Name}»?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            _repo.DeleteCategory(selected.CategoryId);
            LoadData();
        }

        private void RunAction(Action action)
        {
            try
            {
                action();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgres
                && postgres.SqlState == PostgresErrorCodes.ForeignKeyViolation)
            {
                MessageBox.Show(this, "Категория используется учебной группой и не может быть удалена.",
                    "Категории", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка работы с категориями",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
