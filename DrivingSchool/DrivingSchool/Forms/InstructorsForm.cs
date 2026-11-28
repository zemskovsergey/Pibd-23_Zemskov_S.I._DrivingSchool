using System;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class InstructorsForm : Form
    {
        private readonly IInstructorRepository _repo;
        private readonly DataGridView _grid;

        public InstructorsForm(IInstructorRepository repo)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _grid = ConfigureList(this, "Справочник: Инструкторы", new[]
            {
                (nameof(Instructor.FullName), "ФИО", ""),
                (nameof(Instructor.Phone), "Телефон", "")
            }, ("Добавить", () => OpenEditor(null)), ("Редактировать", EditSelected), ("Удалить", DeleteSelected));
            Shown += (_, _) => Run(this, LoadData);
        }

        private void LoadData()
        {
            _grid.DataSource = _repo.ReadInstructors().ToList();
        }

        private void OpenEditor(Instructor existing)
        {
            Action<Instructor> save = existing == null ? _repo.CreateInstructor : _repo.UpdateInstructor;
            using var dialog = new AddInstructorForm(save, existing);
            if (dialog.ShowDialog(this) == DialogResult.OK)
                LoadData();
        }

        private void EditSelected()
        {
            var selected = ShowSelection<Instructor>(this, _grid);
            if (selected == null)
                return;
            var existing = _repo.ReadInstructorById(selected.InstructorId)
                ?? throw new InvalidOperationException("Запись больше не существует.");
            OpenEditor(existing);
        }

        private void DeleteSelected()
        {
            var selected = ShowSelection<Instructor>(this, _grid);
            if (selected == null || MessageBox.Show(this, "Удалить выбранную запись?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            _repo.DeleteInstructor(selected.InstructorId);
            LoadData();
        }
    }
}
