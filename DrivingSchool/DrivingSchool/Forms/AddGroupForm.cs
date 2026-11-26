using System;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Entities.Enums;

namespace DrivingSchool.Forms
{
    public partial class AddGroupForm : Form
    {
        public Group CreatedGroup { get; private set; }

        private TextBox txtGroupName;
        private TextBox txtCategoryId;
        private TextBox txtInstructorId;
        private DateTimePicker dtpStartDate;
        private DateTimePicker dtpEndDate;
        private CheckedListBox clbTrainingDays;
        private Button btnSave;
        private Button btnCancel;

        public AddGroupForm()
        {
            InitializeComponent();
            SetupComponents();
        }

        private void SetupComponents()
        {
            this.Text = "Добавление группы";
            this.Width = 420;
            this.Height = 450; 
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblName = new Label { Text = "Название:", Left = 20, Top = 23, Width = 120 };
            txtGroupName = new TextBox { Left = 150, Top = 20, Width = 220 };

            var lblCat = new Label { Text = "ID категории:", Left = 20, Top = 63, Width = 120 };
            txtCategoryId = new TextBox { Left = 150, Top = 60, Width = 220 };

            var lblInst = new Label { Text = "ID инструктора:", Left = 20, Top = 103, Width = 120 };
            txtInstructorId = new TextBox { Left = 150, Top = 100, Width = 220 };

            var lblStart = new Label { Text = "Дата начала:", Left = 20, Top = 143, Width = 120 };
            dtpStartDate = new DateTimePicker { Left = 150, Top = 140, Width = 220, Format = DateTimePickerFormat.Short };

            var lblEnd = new Label { Text = "Дата окончания:", Left = 20, Top = 183, Width = 120 };
            dtpEndDate = new DateTimePicker { Left = 150, Top = 180, Width = 220, Format = DateTimePickerFormat.Short };

            var lblDays = new Label { Text = "Дни занятий:", Left = 20, Top = 223, Width = 120 };
            clbTrainingDays = new CheckedListBox { Left = 150, Top = 220, Width = 220, Height = 110, CheckOnClick = true };

            foreach (TrainingDaysEnum day in Enum.GetValues(typeof(TrainingDaysEnum)))
            {
                if (day != TrainingDaysEnum.None)
                {
                    clbTrainingDays.Items.Add(day);
                }
            }

            btnSave = new Button { Text = "Сохранить", Left = 150, Top = 350, Width = 105, Height = 30 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Отмена", Left = 265, Top = 350, Width = 105, Height = 30 };
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblName);
            this.Controls.Add(txtGroupName);
            this.Controls.Add(lblCat);
            this.Controls.Add(txtCategoryId);
            this.Controls.Add(lblInst);
            this.Controls.Add(txtInstructorId);
            this.Controls.Add(lblStart);
            this.Controls.Add(dtpStartDate);
            this.Controls.Add(lblEnd);
            this.Controls.Add(dtpEndDate);
            this.Controls.Add(lblDays);
            this.Controls.Add(clbTrainingDays);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGroupName.Text) ||
                string.IsNullOrWhiteSpace(txtCategoryId.Text) ||
                string.IsNullOrWhiteSpace(txtInstructorId.Text))
            {
                MessageBox.Show("Заполните все текстовые поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (clbTrainingDays.CheckedItems.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы один день занятий!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCategoryId.Text, out int categoryId) ||
                !int.TryParse(txtInstructorId.Text, out int instructorId))
            {
                MessageBox.Show("ID категории и ID инструктора должны быть числами.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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