using System;
using System.Windows.Forms;
using DrivingSchool.Entities;

namespace DrivingSchool.Forms
{
    public partial class AddCategoryForm : Form
    {
        public Category CreatedCategory { get; private set; }

        private TextBox txtName;
        private Button btnSave;
        private Button btnCancel;

        private readonly int _categoryId;

        public AddCategoryForm() : this(null)
        {
        }

        public AddCategoryForm(Category category)
        {
            InitializeComponent();
            SetupComponents();

            if (category != null)
            {
                _categoryId = category.CategoryId;
                Text = "Редактировать категорию";
                txtName.Text = category.Name;
            }
        }

        private void SetupComponents()
        {
            this.Text = "Добавить категорию";
            this.Width = 380;
            this.Height = 180;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lbl = new Label { Text = "Название:", Left = 20, Top = 23, Width = 80 };
            txtName = new TextBox { Left = 110, Top = 20, Width = 230 };

            btnSave = new Button { Text = "Сохранить", Left = 110, Top = 80, Width = 110, Height = 30 };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button { Text = "Отмена", Left = 230, Top = 80, Width = 110, Height = 30 };
            btnCancel.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lbl);
            this.Controls.Add(txtName);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Заполните название категории!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CreatedCategory = Category.CreateEntity(_categoryId, txtName.Text.Trim());
            DialogResult = DialogResult.OK;
        }
    }
}