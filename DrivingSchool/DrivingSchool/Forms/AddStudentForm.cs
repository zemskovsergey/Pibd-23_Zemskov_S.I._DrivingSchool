using System;
using System.Windows.Forms;
using DrivingSchool.Entities;

namespace DrivingSchool.Forms
{
    public partial class AddStudentForm : Form
    {
        public Student CreatedStudent { get; private set; }

        private TextBox txtFullName;
        private TextBox txtPhone;
        private TextBox txtGroupId;
        private TextBox txtPayment;
        private Button btnSave;
        private Button btnCancel;

        public AddStudentForm()
        {
            InitializeComponent();
            SetupComponents();
        }

        private void SetupComponents()
        {
            this.Text = "Добавление студента";
            this.Width = 400;
            this.Height = 290;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            var lblName = new Label { Text = "ФИО:", Left = 20, Top = 23, Width = 100 };
            txtFullName = new TextBox { Left = 130, Top = 20, Width = 230 };

            var lblPhone = new Label { Text = "Телефон:", Left = 20, Top = 63, Width = 100 };
            txtPhone = new TextBox { Left = 130, Top = 60, Width = 230 };

            var lblGroup = new Label { Text = "ID группы:", Left = 20, Top = 103, Width = 100 };
            txtGroupId = new TextBox { Left = 130, Top = 100, Width = 230 };

            var lblPayment = new Label { Text = "Оплата (руб.):", Left = 20, Top = 143, Width = 100 };
            txtPayment = new TextBox { Left = 130, Top = 140, Width = 230 };

            btnSave = new Button { Text = "Сохранить", Left = 130, Top = 195, Width = 110, Height = 30 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Отмена", Left = 250, Top = 195, Width = 110, Height = 30 };
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblName);
            this.Controls.Add(txtFullName);
            this.Controls.Add(lblPhone);
            this.Controls.Add(txtPhone);
            this.Controls.Add(lblGroup);
            this.Controls.Add(txtGroupId);
            this.Controls.Add(lblPayment);
            this.Controls.Add(txtPayment);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtGroupId.Text) ||
                string.IsNullOrWhiteSpace(txtPayment.Text))
            {
                MessageBox.Show("Заполните все обязательные поля!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Проверка корректности числовых значений
            if (!int.TryParse(txtGroupId.Text, out int groupId) || groupId <= 0)
            {
                MessageBox.Show("ID группы должен быть положительным целым числом.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPayment.Text, out decimal paymentAmount) || paymentAmount < 0)
            {
                MessageBox.Show("Сумма оплаты должна быть корректным числом.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}