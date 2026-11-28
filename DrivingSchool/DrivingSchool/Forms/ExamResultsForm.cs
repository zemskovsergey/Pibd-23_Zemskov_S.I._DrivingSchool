using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class ExamResultsForm : Form
    {
        private readonly IExamResultRepository _repo;
        private readonly IStudentRepository _students;
        private readonly DataGridView _grid;
        private Dictionary<int, string> _StudentIdNames = new();

        public ExamResultsForm(IExamResultRepository repo, IStudentRepository students)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _students = students ?? throw new ArgumentNullException(nameof(students));
            _grid = ConfigureList(this, "Журнал: Результаты экзаменов", new[]
            {
                (nameof(ExamResult.StudentId), "Студент", ""),
                (nameof(ExamResult.TheoryScore), "Теория", ""),
                (nameof(ExamResult.DrivingScore), "Вождение", ""),
                (nameof(ExamResult.ExamDate), "Дата экзамена", "d")
            }, ("Добавить", OpenEditor));
            _grid.CellFormatting += FormatCell;
            Shown += (_, _) => Run(this, LoadData);
        }

        private void LoadData()
        {
            _StudentIdNames = _students.ReadStudents().ToDictionary(x => x.StudentId, x => x.FullName);
            _grid.DataSource = _repo.ReadExamResults().ToList();
        }

        private void FormatCell(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var value = _grid.Columns[e.ColumnIndex].DataPropertyName switch
            {
                nameof(ExamResult.StudentId) when e.Value is int id => _StudentIdNames.GetValueOrDefault(id, $"№{id}"),
                _ => null
            };
            if (value != null)
            {
                e.Value = value;
                e.FormattingApplied = true;
            }
        }

        private void OpenEditor()
        {
            using var dialog = new AddExamResultForm(_students, _repo.CreateExamResult);
            if (dialog.ShowDialog(this) == DialogResult.OK)
                LoadData();
        }
    }
}
