using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class StudentsForm : Form
    {
        private readonly IStudentRepository _repo;
        private readonly IGroupRepository _groups;
        private readonly DataGridView _grid;
        private Dictionary<int, string> _GroupIdNames = new();

        public StudentsForm(IStudentRepository repo, IGroupRepository groups)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _groups = groups ?? throw new ArgumentNullException(nameof(groups));
            _grid = ConfigureList(this, "Справочник: Студенты", new[]
            {
                (nameof(Student.FullName), "ФИО", ""),
                (nameof(Student.Phone), "Телефон", ""),
                (nameof(Student.GroupId), "Группа", ""),
                (nameof(Student.TuitionFee), "Стоимость, ₽", "N2")
            }, ("Добавить", () => OpenEditor(null)), ("Редактировать", EditSelected), ("Удалить", DeleteSelected));
            _grid.CellFormatting += FormatCell;
            Shown += (_, _) => Run(this, LoadData);
        }

        private void LoadData()
        {
            _GroupIdNames = _groups.ReadGroups().ToDictionary(x => x.GroupId, x => x.GroupName);
            _grid.DataSource = _repo.ReadStudents().ToList();
        }

        private void FormatCell(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var value = _grid.Columns[e.ColumnIndex].DataPropertyName switch
            {
                nameof(Student.GroupId) when e.Value is int id => _GroupIdNames.GetValueOrDefault(id, $"№{id}"),
                _ => null
            };
            if (value != null)
            {
                e.Value = value;
                e.FormattingApplied = true;
            }
        }

        private void OpenEditor(Student existing)
        {
            Action<Student> save = existing == null ? _repo.CreateStudent : _repo.UpdateStudent;
            using var dialog = new AddStudentForm(_groups, save, existing);
            if (dialog.ShowDialog(this) == DialogResult.OK)
                LoadData();
        }

        private void EditSelected()
        {
            var selected = ShowSelection<Student>(this, _grid);
            if (selected == null)
                return;
            var existing = _repo.ReadStudentById(selected.StudentId)
                ?? throw new InvalidOperationException("Запись больше не существует.");
            OpenEditor(existing);
        }

        private void DeleteSelected()
        {
            var selected = ShowSelection<Student>(this, _grid);
            if (selected == null || MessageBox.Show(this, "Удалить выбранную запись?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            _repo.DeleteStudent(selected.StudentId);
            LoadData();
        }
    }
}
