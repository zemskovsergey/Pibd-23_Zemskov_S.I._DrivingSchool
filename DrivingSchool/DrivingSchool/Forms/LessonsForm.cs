using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class LessonsForm : Form
    {
        private readonly ILessonRepository _repo;
        private readonly IGroupRepository _groups;
        private readonly IInstructorRepository _instructors;
        private readonly DataGridView _grid;
        private Dictionary<int, string> _GroupIdNames = new();
        private Dictionary<int, string> _InstructorIdNames = new();

        public LessonsForm(ILessonRepository repo, IGroupRepository groups, IInstructorRepository instructors)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _groups = groups ?? throw new ArgumentNullException(nameof(groups));
            _instructors = instructors ?? throw new ArgumentNullException(nameof(instructors));
            _grid = ConfigureList(this, "Журнал: Занятия", new[]
            {
                (nameof(Lesson.LessonDate), "Дата", "d"),
                (nameof(Lesson.LessonType), "Тип / тема", ""),
                (nameof(Lesson.GroupId), "Группа", ""),
                (nameof(Lesson.InstructorId), "Инструктор", "")
            }, ("Добавить", OpenEditor));
            _grid.CellFormatting += FormatCell;
            Shown += (_, _) => Run(this, LoadData);
        }

        private void LoadData()
        {
            _GroupIdNames = _groups.ReadGroups().ToDictionary(x => x.GroupId, x => x.GroupName);
            _InstructorIdNames = _instructors.ReadInstructors().ToDictionary(x => x.InstructorId, x => x.FullName);
            _grid.DataSource = _repo.ReadLessons().ToList();
        }

        private void FormatCell(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var value = _grid.Columns[e.ColumnIndex].DataPropertyName switch
            {
                nameof(Lesson.GroupId) when e.Value is int id => _GroupIdNames.GetValueOrDefault(id, $"№{id}"),
                nameof(Lesson.InstructorId) when e.Value is int id => _InstructorIdNames.GetValueOrDefault(id, $"№{id}"),
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
            using var dialog = new AddLessonForm(_groups, _instructors, _repo.CreateLesson);
            if (dialog.ShowDialog(this) == DialogResult.OK)
                LoadData();
        }
    }
}
