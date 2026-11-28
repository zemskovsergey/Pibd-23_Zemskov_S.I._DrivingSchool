using System;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Entities.Enums;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class AddGroupForm : Form
    {
        public AddGroupForm(ICategoryRepository categories, IInstructorRepository instructors, Action<Group> save, Group existing = null)
        {
            InitializeComponent();
            var name = new TextBox { Text = existing?.GroupName ?? string.Empty };
            var category = Choice(categories.ReadCategories().Select(x => (x.CategoryId, x.Name)), existing?.CategoryId);
            var instructor = Choice(instructors.ReadInstructors().Select(x => (x.InstructorId, $"{x.FullName} (№{x.InstructorId})")), existing?.InstructorId);
            var start = DatePicker(existing?.StartDate);
            var end = DatePicker(existing?.EndDate);
            var days = new CheckedListBox { CheckOnClick = true, Height = 110 };
            var schedule = TrainingDays.ToArray();
            foreach (var (flag, caption) in schedule)
                days.Items.Add(caption, existing?.TrainingDays.HasFlag(flag) == true);
            ConfigureEditor(this, existing == null ? "Добавить группу" : "Редактировать группу", () =>
            {
                if (end.Value.Date < start.Value.Date)
                    throw new ArgumentException("Окончание обучения не может быть раньше начала.");
                var flags = TrainingDaysEnum.None;
                foreach (int index in days.CheckedIndices)
                    flags |= schedule[index].Flag;
                if (flags == TrainingDaysEnum.None)
                    throw new ArgumentException("Выберите хотя бы один день занятий.");
                var entity = Group.CreateEntity(existing?.GroupId ?? 0, RequiredText(name, "Введите название группы."),
                    RequiredChoice(category, "Выберите категорию. Если список пуст, сначала добавьте категорию."),
                    RequiredChoice(instructor, "Выберите инструктора. Если список пуст, сначала добавьте инструктора."),
                    start.Value.Date, end.Value.Date, flags);
                save(entity);
            }, ("Название", name), ("Категория", category), ("Инструктор", instructor), ("Начало обучения", start),
                ("Окончание обучения", end), ("Дни занятий", days));
        }
    }
}
