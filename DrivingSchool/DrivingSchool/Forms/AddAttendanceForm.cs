using System;
using System.Windows.Forms;
using DrivingSchool.Entities;

namespace DrivingSchool.Forms
{
    public partial class AddAttendanceForm : Form
    {
        public Attendance CreatedAttendance { get; private set; }

        private TextBox txtStudentId;
        private TextBox txtLessonId;
        private CheckBox chkIsPresent;
        private Button btnSave;
        private Button btnCancel;

        public AddAttendanceForm()
        {
            InitializeComponent();
            SetupComponents();
        }

        private void SetupComponents()
        {
            this.Text = "Отметка посещаемости";
            this.Width = 380;
            this.Height = 240;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblStudent = new Label { Text = "ID Студента:", Left = 20, Top = 23, Width = 100 };
            txtStudentId = new TextBox { Left = 130, Top = 20, Width = 200 };

            var lblLesson = new Label { Text = "ID Занятия:", Left = 20, Top = 63, Width = 100 };
            txtLessonId = new TextBox { Left = 130, Top = 60, Width = 200 };

            chkIsPresent = new CheckBox { Text = "Присутствовал", Left = 130, Top = 105, Width = 200 };

            btnSave = new Button { Text = "Сохранить", Left = 130, Top = 145, Width = 95, Height = 30 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Отмена", Left = 235, Top = 145, Width = 95, Height = 30 };
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblStudent);
            this.Controls.Add(txtStudentId);
            this.Controls.Add(lblLesson);
            this.Controls.Add(txtLessonId);
            this.Controls.Add(chkIsPresent);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtStudentId.Text, out int studentId) || !int.TryParse(txtLessonId.Text, out int lessonId))
            {
                MessageBox.Show("ID студента и ID занятия должны быть числами.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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