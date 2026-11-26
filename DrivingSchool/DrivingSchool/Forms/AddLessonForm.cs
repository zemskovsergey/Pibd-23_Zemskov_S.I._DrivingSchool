using System;
using System.Windows.Forms;
using DrivingSchool.Entities;

namespace DrivingSchool.Forms
{
    public partial class AddLessonForm : Form
    {
        public Lesson CreatedLesson { get; private set; }

        private TextBox txtLessonType;
        private TextBox txtGroupId;
        private TextBox txtInstructorId;
        private DateTimePicker dtpDate;
        private Button btnSave;
        private Button btnCancel;

        public AddLessonForm()
        {
            InitializeComponent();
            SetupComponents();
        }

        private void SetupComponents()
        {
            this.Text = "Добавление занятия";
            this.Width = 400;
            this.Height = 260;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblType = new Label { Text = "Тип / Тема:", Left = 20, Top = 23, Width = 110 };
            txtLessonType = new TextBox { Left = 140, Top = 20, Width = 210 };

            var lblGroup = new Label { Text = "ID группы:", Left = 20, Top = 63, Width = 110 };
            txtGroupId = new TextBox { Left = 140, Top = 60, Width = 210 };

            var lblInst = new Label { Text = "ID инструктора:", Left = 20, Top = 103, Width = 110 };
            txtInstructorId = new TextBox { Left = 140, Top = 100, Width = 210 };

            var lblDate = new Label { Text = "Дата занятия:", Left = 20, Top = 143, Width = 110 };
            dtpDate = new DateTimePicker { Left = 140, Top = 140, Width = 210, Format = DateTimePickerFormat.Short };

            btnSave = new Button { Text = "Сохранить", Left = 140, Top = 185, Width = 100, Height = 30 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Отмена", Left = 250, Top = 185, Width = 100, Height = 30 };
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblType);
            this.Controls.Add(txtLessonType);
            this.Controls.Add(lblGroup);
            this.Controls.Add(txtGroupId);
            this.Controls.Add(lblInst);
            this.Controls.Add(txtInstructorId);
            this.Controls.Add(lblDate);
            this.Controls.Add(dtpDate);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLessonType.Text) ||
                !int.TryParse(txtGroupId.Text, out int groupId) ||
                !int.TryParse(txtInstructorId.Text, out int instructorId))
            {
                MessageBox.Show("Проверьте правильность заполнения полей.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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