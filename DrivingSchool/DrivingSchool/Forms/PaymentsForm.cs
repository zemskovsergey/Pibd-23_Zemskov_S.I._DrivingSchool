using System;
using System.Windows.Forms;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;

namespace DrivingSchool.Forms
{
    public partial class PaymentsForm : Form
    {
        private readonly IPaymentRepository _paymentRepository;
        private DataGridView dataGridViewPayments;
        private Button btnAdd;

        public PaymentsForm(IPaymentRepository paymentRepository)
        {
            InitializeComponent();
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));

            SetupForm();
            LoadData();
        }

        private void SetupForm()
        {
            this.Text = "Журнал: Оплаты";
            this.Width = 750;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterParent;

            btnAdd = new Button
            {
                Text = "Добавить платеж",
                Dock = DockStyle.Top,
                Height = 35
            };
            btnAdd.Click += BtnAdd_Click;

            dataGridViewPayments = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            this.Controls.Add(dataGridViewPayments);
            this.Controls.Add(btnAdd);
        }

        private void LoadData()
        {
            dataGridViewPayments.DataSource = null;
            dataGridViewPayments.DataSource = _paymentRepository.ReadPayments();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var addForm = new AddPaymentForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK && addForm.CreatedPayment != null)
                {
                    _paymentRepository.CreatePayment(addForm.CreatedPayment);
                    LoadData();
                }
            }
        }
    }
}