using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class AddPaymentForm : Form
    {
        private ComboBox cbStudent;
        private TextBox txtAmount;
        private DateTimePicker dtpDate;
        private Button btnSave;
        private Button btnCancel;
        private readonly Action<Payment> _save;

        public AddPaymentForm(IStudentRepository students, Action<Payment> save)
        {
            _save = save ?? throw new ArgumentNullException(nameof(save));
            cbStudent = Choice(students.ReadStudents().Select(x => (x.StudentId, $"{x.FullName} (№{x.StudentId})")));
            InitializeComponent();
            SetupComponents();
        }

        private void SetupComponents()
        {
            this.Text = "Регистрация оплаты";
            this.ClientSize = new Size(390, 215);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblStudent = new Label { Text = "Студент:", Left = 20, Top = 23, Width = 100 };
            cbStudent.SetBounds(130, 20, 230, cbStudent.PreferredHeight);

            var lblAmount = new Label { Text = "Сумма (руб.):", Left = 20, Top = 63, Width = 100 };
            txtAmount = new TextBox { Left = 130, Top = 60, Width = 230 };

            var lblDate = new Label { Text = "Дата оплаты:", Left = 20, Top = 103, Width = 100 };
            dtpDate = DatePicker();
            dtpDate.SetBounds(130, 100, 230, dtpDate.Height);

            btnSave = new Button { Text = "Сохранить", Left = 130, Top = 155, Width = 110, Height = 30 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Отмена", Left = 250, Top = 155, Width = 110, Height = 30 };
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblStudent);
            this.Controls.Add(cbStudent);
            this.Controls.Add(lblAmount);
            this.Controls.Add(txtAmount);
            this.Controls.Add(lblDate);
            this.Controls.Add(dtpDate);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
            AcceptButton = btnSave;
            CancelButton = btnCancel;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (Run(this, () =>
            {
                var studentId = RequiredChoice(cbStudent, "Выберите студента. Если список пуст, сначала добавьте студента.");
                var amount = Amount(txtAmount, false, "Укажите положительную сумму оплаты с точностью до копеек.");
                _save(Payment.CreateEntity(0, studentId, amount, dtpDate.Value.Date));
            }))
                DialogResult = DialogResult.OK;
        }
    }
}
