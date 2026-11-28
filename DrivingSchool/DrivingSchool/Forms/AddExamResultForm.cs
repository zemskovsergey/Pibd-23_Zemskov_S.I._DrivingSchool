using System;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class AddExamResultForm : Form
    {
        public AddExamResultForm(IStudentRepository students, Action<ExamResult> save)
        {
            InitializeComponent();
            var student = Choice(students.ReadStudents().Select(x => (x.StudentId, $"{x.FullName} (№{x.StudentId})")));
            var theory = new TextBox();
            var driving = new TextBox();
            var date = DatePicker();
            ConfigureEditor(this, "Записать результат экзамена", () =>
            {
                var entity = ExamResult.CreateEntity(0,
                    RequiredChoice(student, "Выберите студента. Если список пуст, сначала добавьте студента."),
                    RequiredText(theory, "Введите результат по теории."), RequiredText(driving, "Введите результат по вождению."),
                    date.Value.Date);
                save(entity);
            }, ("Студент", student), ("Теория", theory), ("Вождение", driving), ("Дата экзамена", date));
        }
    }
}
