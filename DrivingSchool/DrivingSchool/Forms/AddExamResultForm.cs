using System;
using System.Windows.Forms;
using DrivingSchool.Entities;

namespace DrivingSchool.Forms
{
    public partial class AddExamResultForm : Form
    {
        public ExamResult CreatedExamResult { get; private set; }

        private TextBox txtStudentId;
        private TextBox txtTheoryScore;
        private TextBox txtDrivingScore;
        private DateTimePicker dtpExamDate;
        private Button btnSave;
        private Button btnCancel;

        public AddExamResultForm()
        {
            InitializeComponent();
            SetupComponents();
        }

        private void SetupComponents()
        {
            this.Text = "Результат экзамена";
            this.Width = 400;
            this.Height = 280;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblStudent = new Label { Text = "ID Студента:", Left = 20, Top = 23, Width = 120 };
            txtStudentId = new TextBox { Left = 150, Top = 20, Width = 210 };

            var lblTheory = new Label { Text = "Теория:", Left = 20, Top = 63, Width = 120 };
            txtTheoryScore = new TextBox { Left = 150, Top = 60, Width = 210 };

            var lblDriving = new Label { Text = "Вождение:", Left = 20, Top = 103, Width = 120 };
            txtDrivingScore = new TextBox { Left = 150, Top = 100, Width = 210 };

            var lblDate = new Label { Text = "Дата экзамена:", Left = 20, Top = 143, Width = 120 };
            dtpExamDate = new DateTimePicker { Left = 150, Top = 140, Width = 210, Format = DateTimePickerFormat.Short };

            btnSave = new Button { Text = "Сохранить", Left = 150, Top = 195, Width = 100, Height = 30 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Отмена", Left = 260, Top = 195, Width = 100, Height = 30 };
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblStudent);
            this.Controls.Add(txtStudentId);
            this.Controls.Add(lblTheory);
            this.Controls.Add(txtTheoryScore);
            this.Controls.Add(lblDriving);
            this.Controls.Add(txtDrivingScore);
            this.Controls.Add(lblDate);
            this.Controls.Add(dtpExamDate);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtStudentId.Text, out int studentId))
            {
                MessageBox.Show("ID студента должен быть числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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