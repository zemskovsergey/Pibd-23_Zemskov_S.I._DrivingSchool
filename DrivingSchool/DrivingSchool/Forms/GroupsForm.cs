using System;
using System.Collections.Generic;
using DrivingSchool.Entities.Enums;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class GroupsForm : Form
    {
        private readonly IGroupRepository _repo;
        private readonly ICategoryRepository _categories;
        private readonly IInstructorRepository _instructors;
        private readonly DataGridView _grid;
        private Dictionary<int, string> _CategoryIdNames = new();
        private Dictionary<int, string> _InstructorIdNames = new();

        public GroupsForm(IGroupRepository repo, ICategoryRepository categories, IInstructorRepository instructors)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _categories = categories ?? throw new ArgumentNullException(nameof(categories));
            _instructors = instructors ?? throw new ArgumentNullException(nameof(instructors));
            _grid = ConfigureList(this, "Справочник: Учебные группы", new[]
            {
                (nameof(Group.GroupName), "Название", ""),
                (nameof(Group.CategoryId), "Категория", ""),
                (nameof(Group.InstructorId), "Инструктор", ""),
                (nameof(Group.StartDate), "Начало", "d"),
                (nameof(Group.EndDate), "Окончание", "d"),
                (nameof(Group.TrainingDays), "Дни занятий", "")
            }, ("Добавить", () => OpenEditor(null)), ("Редактировать", EditSelected), ("Удалить", DeleteSelected));
            _grid.CellFormatting += FormatCell;
            Shown += (_, _) => Run(this, LoadData);
        }

        private void LoadData()
        {
            _CategoryIdNames = _categories.ReadCategories().ToDictionary(x => x.CategoryId, x => x.Name);
            _InstructorIdNames = _instructors.ReadInstructors().ToDictionary(x => x.InstructorId, x => x.FullName);
            _grid.DataSource = _repo.ReadGroups().ToList();
        }

        private void FormatCell(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var value = _grid.Columns[e.ColumnIndex].DataPropertyName switch
            {
                nameof(Group.CategoryId) when e.Value is int id => _CategoryIdNames.GetValueOrDefault(id, $"№{id}"),
                nameof(Group.InstructorId) when e.Value is int id => _InstructorIdNames.GetValueOrDefault(id, $"№{id}"),
                nameof(Group.TrainingDays) when e.Value is TrainingDaysEnum flags => FormatTrainingDays(flags),
                _ => null
            };
            if (value != null)
            {
                e.Value = value;
                e.FormattingApplied = true;
            }
        }

        private void OpenEditor(Group existing)
        {
            Action<Group> save = existing == null ? _repo.CreateGroup : _repo.UpdateGroup;
            using var dialog = new AddGroupForm(_categories, _instructors, save, existing);
            if (dialog.ShowDialog(this) == DialogResult.OK)
                LoadData();
        }

        private void EditSelected()
        {
            var selected = ShowSelection<Group>(this, _grid);
            if (selected == null)
                return;
            var existing = _repo.ReadGroupById(selected.GroupId)
                ?? throw new InvalidOperationException("Запись больше не существует.");
            OpenEditor(existing);
        }

        private void DeleteSelected()
        {
            var selected = ShowSelection<Group>(this, _grid);
            if (selected == null || MessageBox.Show(this, "Удалить выбранную запись?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            _repo.DeleteGroup(selected.GroupId);
            LoadData();
        }
    }
}
