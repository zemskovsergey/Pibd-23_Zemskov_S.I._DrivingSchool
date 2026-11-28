using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class AttendanceForm : Form
    {
        private readonly IAttendanceRepository _repo;
        private readonly IStudentRepository _students;
        private readonly ILessonRepository _lessons;
        private readonly IGroupRepository _groups;
        private readonly DataGridView _grid;
        private Dictionary<int, string> _StudentIdNames = new();
        private Dictionary<int, string> _LessonIdNames = new();

        public AttendanceForm(IAttendanceRepository repo, IStudentRepository students, ILessonRepository lessons, IGroupRepository groups)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _students = students ?? throw new ArgumentNullException(nameof(students));
            _lessons = lessons ?? throw new ArgumentNullException(nameof(lessons));
            _groups = groups ?? throw new ArgumentNullException(nameof(groups));
            _grid = ConfigureList(this, "Журнал: Посещаемость", new[]
            {
                (nameof(Attendance.StudentId), "Студент", ""),
                (nameof(Attendance.LessonId), "Занятие", ""),
                (nameof(Attendance.IsPresent), "Отметка", "")
            }, ("Добавить", OpenEditor));
            _grid.CellFormatting += FormatCell;
            Shown += (_, _) => Run(this, LoadData);
        }

        private void LoadData()
        {
            _StudentIdNames = _students.ReadStudents().ToDictionary(x => x.StudentId, x => x.FullName);
            _LessonIdNames = _lessons.ReadLessons().ToDictionary(x => x.LessonId, x => $"{x.LessonDate:d} — {x.LessonType} (№{x.LessonId})");
            _grid.DataSource = _repo.ReadAttendances().ToList();
        }

        private void FormatCell(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var value = _grid.Columns[e.ColumnIndex].DataPropertyName switch
            {
                nameof(Attendance.StudentId) when e.Value is int id => _StudentIdNames.GetValueOrDefault(id, $"№{id}"),
                nameof(Attendance.LessonId) when e.Value is int id => _LessonIdNames.GetValueOrDefault(id, $"№{id}"),
                nameof(Attendance.IsPresent) when e.Value is bool present => present ? "Присутствовал" : "Отсутствовал",
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
            using var dialog = new AddAttendanceForm(_students, _lessons, _groups, _repo.CreateAttendance);
            if (dialog.ShowDialog(this) == DialogResult.OK)
                LoadData();
        }
    }
}
