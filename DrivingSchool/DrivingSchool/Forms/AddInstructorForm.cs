using System;
using System.Windows.Forms;
using DrivingSchool.Entities;

namespace DrivingSchool.Forms
{
    public partial class AddInstructorForm : Form
    {
        public Instructor CreatedInstructor { get; private set; }

        private TextBox txtFullName;
        private TextBox txtPhone;
        private Button btnSave;
        private Button btnCancel;

        public AddInstructorForm()
        {
            InitializeComponent();
            SetupComponents();
        }

        private void SetupComponents()
        {
            this.Text = "Добавление преподавателя";
            this.Width = 400;
            this.Height = 220;
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
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}