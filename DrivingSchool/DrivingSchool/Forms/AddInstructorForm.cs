using System;
using System.Drawing;
using System.Windows.Forms;
using DrivingSchool.Entities;
using static DrivingSchool.Forms.FormHelpers;

namespace DrivingSchool.Forms
{
    public partial class AddInstructorForm : Form
    {
        private TextBox txtFullName;
        private TextBox txtPhone;
        private Button btnSave;
        private Button btnCancel;
        private readonly Action<Instructor> _save;
        private readonly int _instructorId;

        public AddInstructorForm(Action<Instructor> save, Instructor existing = null)
        {
            _save = save ?? throw new ArgumentNullException(nameof(save));
            InitializeComponent();
            SetupComponents();
            if (existing != null)
            {
                _instructorId = existing.InstructorId;
                Text = "Редактировать инструктора";
                txtFullName.Text = existing.FullName;
                txtPhone.Text = existing.Phone;
            }
        }

        private void SetupComponents()
        {
            this.Text = "Добавление инструктора";
            this.ClientSize = new Size(384, 170);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var lblName = new Label { Text = "ФИО:", Left = 20, Top = 23, Width = 100 };
            txtFullName = new TextBox { Left = 130, Top = 20, Width = 230 };

            var lblPhone = new Label { Text = "Телефон:", Left = 20, Top = 63, Width = 100 };
            txtPhone = new TextBox { Left = 130, Top = 60, Width = 230 };

            btnSave = new Button { Text = "Сохранить", Left = 130, Top = 115, Width = 110, Height = 30 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Отмена", Left = 250, Top = 115, Width = 110, Height = 30 };
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblName);
            this.Controls.Add(txtFullName);
            this.Controls.Add(lblPhone);
            this.Controls.Add(txtPhone);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
            AcceptButton = btnSave;
            CancelButton = btnCancel;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Run(this, () => _save(Instructor.CreateEntity(_instructorId, txtFullName.Text.Trim(), txtPhone.Text.Trim()))))
                DialogResult = DialogResult.OK;
        }
    }
}
