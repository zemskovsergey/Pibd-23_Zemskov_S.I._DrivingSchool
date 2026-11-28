using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class AddAttendanceForm : Form
    {
        public AddAttendanceForm(IStudentRepository students, ILessonRepository lessons, IGroupRepository groups, Action<Attendance> save)
        {
            InitializeComponent();
            var studentRows = students.ReadStudents().ToList();
            var lessonRows = lessons.ReadLessons().ToDictionary(x => x.LessonId);
            var groupNames = groups.ReadGroups().ToDictionary(x => x.GroupId, x => x.GroupName);
            var lesson = Choice(lessonRows.Values.Select(x => (x.LessonId,
                $"{x.LessonDate:d} — {x.LessonType} — {groupNames.GetValueOrDefault(x.GroupId, $"№{x.GroupId}")} (№{x.LessonId})")));
            var student = Choice(Array.Empty<(int, string)>());
            var present = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            present.Items.AddRange(new object[] { "Присутствовал", "Отсутствовал" });
            lesson.SelectedIndexChanged += (_, _) =>
            {
                var candidates = lesson.SelectedValue is int id && lessonRows.TryGetValue(id, out var selected)
                    ? studentRows.Where(x => x.GroupId == selected.GroupId)
                    : Enumerable.Empty<Student>();
                BindChoices(student, candidates.Select(x => (x.StudentId, $"{x.FullName} (№{x.StudentId})")));
            };
            ConfigureEditor(this, "Отметить посещаемость", () =>
            {
                var lessonId = RequiredChoice(lesson, "Выберите занятие. Если список пуст, сначала добавьте занятие.");
                var studentId = RequiredChoice(student, "Выберите студента из группы этого занятия.");
                if (present.SelectedIndex < 0)
                    throw new ArgumentException("Укажите, присутствовал ли студент.");
                save(Attendance.CreateEntity(0, studentId, present.SelectedIndex == 0, lessonId));
            }, ("Занятие", lesson), ("Студент", student), ("Отметка", present));
        }
    }
}
