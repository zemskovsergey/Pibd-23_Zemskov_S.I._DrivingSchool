using System;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class AddStudentForm : Form
    {
        public AddStudentForm(IGroupRepository groups, Action<Student> save, Student existing = null)
        {
            InitializeComponent();
            var name = new TextBox { Text = existing?.FullName ?? string.Empty };
            var phone = new TextBox { Text = existing?.Phone ?? string.Empty };
            var group = Choice(groups.ReadGroups().Select(x => (x.GroupId, $"{x.GroupName} (№{x.GroupId})")), existing?.GroupId);
            var fee = new TextBox { Text = existing?.TuitionFee.ToString() ?? string.Empty };
            ConfigureEditor(this, existing == null ? "Добавить студента" : "Редактировать студента", () =>
            {
                var entity = Student.CreateEntity(existing?.StudentId ?? 0,
                    RequiredText(name, "Введите ФИО студента."), RequiredText(phone, "Введите телефон студента."),
                    RequiredChoice(group, "Выберите учебную группу. Если список пуст, сначала создайте группу."),
                    Amount(fee, true, "Укажите неотрицательную стоимость обучения с точностью до копеек."));
                save(entity);
            }, ("ФИО", name), ("Телефон", phone), ("Учебная группа", group), ("Стоимость обучения, ₽", fee));
        }
    }
}
