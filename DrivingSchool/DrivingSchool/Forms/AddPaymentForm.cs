using System;
using System.Windows.Forms;
using DrivingSchool.Entities;

namespace DrivingSchool.Forms
{
    public partial class AddPaymentForm : Form
    {
        public Payment CreatedPayment { get; private set; }

        private TextBox txtStudentId;
        private TextBox txtAmount;
        private Button btnSave;
        private Button btnCancel;

        public AddPaymentForm()
        {
            InitializeComponent();
            SetupComponents();
        }

        private void SetupComponents()
        {
            this.Text = "Регистрация оплаты";
            this.Width = 380;
            this.Height = 220;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblStudent = new Label { Text = "ID Студента:", Left = 20, Top = 23, Width = 100 };
            txtStudentId = new TextBox { Left = 130, Top = 20, Width = 200 };

            var lblAmount = new Label { Text = "Сумма (руб.):", Left = 20, Top = 63, Width = 100 };
            txtAmount = new TextBox { Left = 130, Top = 60, Width = 200 };

            btnSave = new Button { Text = "Сохранить", Left = 130, Top = 115, Width = 95, Height = 30 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Отмена", Left = 235, Top = 115, Width = 95, Height = 30 };
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblStudent);
            this.Controls.Add(txtStudentId);
            this.Controls.Add(lblAmount);
            this.Controls.Add(txtAmount);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentId.Text) || string.IsNullOrWhiteSpace(txtAmount.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtStudentId.Text, out int studentId) || studentId <= 0)
            {
                MessageBox.Show("ID студента должен быть положительным числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount < 0)
            {
                MessageBox.Show("Сумма указана некорректно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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