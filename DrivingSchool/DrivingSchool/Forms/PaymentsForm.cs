using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class PaymentsForm : Form
    {
        private readonly IPaymentRepository _repo;
        private readonly IStudentRepository _students;
        private readonly DataGridView _grid;
        private Dictionary<int, string> _StudentIdNames = new();

        public PaymentsForm(IPaymentRepository repo, IStudentRepository students)
        {
            InitializeComponent();
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _students = students ?? throw new ArgumentNullException(nameof(students));
            _grid = ConfigureList(this, "Журнал: Оплаты", new[]
            {
                (nameof(Payment.StudentId), "Студент", ""),
                (nameof(Payment.Amount), "Сумма, ₽", "N2"),
                (nameof(Payment.PaymentDate), "Дата оплаты", "d")
            }, ("Добавить", OpenEditor));
            _grid.CellFormatting += FormatCell;
            Shown += (_, _) => Run(this, LoadData);
        }

        private void LoadData()
        {
            _StudentIdNames = _students.ReadStudents().ToDictionary(x => x.StudentId, x => x.FullName);
            _grid.DataSource = _repo.ReadPayments().ToList();
        }

        private void FormatCell(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var value = _grid.Columns[e.ColumnIndex].DataPropertyName switch
            {
                nameof(Payment.StudentId) when e.Value is int id => _StudentIdNames.GetValueOrDefault(id, $"№{id}"),
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
            using var dialog = new AddPaymentForm(_students, _repo.CreatePayment);
            if (dialog.ShowDialog(this) == DialogResult.OK)
                LoadData();
        }
    }
}
