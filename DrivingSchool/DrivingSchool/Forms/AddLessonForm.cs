using System;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class AddLessonForm : Form
    {
        public AddLessonForm(IGroupRepository groups, IInstructorRepository instructors, Action<Lesson> save)
        {
            InitializeComponent();
            var group = Choice(groups.ReadGroups().Select(x => (x.GroupId, $"{x.GroupName} (№{x.GroupId})")));
            var instructor = Choice(instructors.ReadInstructors().Select(x => (x.InstructorId, $"{x.FullName} (№{x.InstructorId})")));
            var date = DatePicker();
            var type = new TextBox();
            ConfigureEditor(this, "Добавить занятие", () =>
            {
                var entity = Lesson.CreateEntity(0,
                    RequiredChoice(group, "Выберите учебную группу. Если список пуст, сначала создайте группу."),
                    RequiredChoice(instructor, "Выберите инструктора. Если список пуст, сначала добавьте инструктора."),
                    date.Value.Date, RequiredText(type, "Введите тип или тему занятия."));
                save(entity);
            }, ("Учебная группа", group), ("Инструктор", instructor), ("Дата занятия", date), ("Тип / тема", type));
        }
    }
}
