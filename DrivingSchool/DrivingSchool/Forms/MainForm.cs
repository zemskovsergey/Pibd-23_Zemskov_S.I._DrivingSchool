using System;
using System.Windows.Forms;
using DrivingSchool.Repositories;
using DrivingSchool.Forms;

namespace DrivingSchool
{
    public partial class MainForm : Form
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IExamResultRepository _examResultRepository;

        public MainForm(
            IStudentRepository studentRepository,
            IInstructorRepository instructorRepository,
            IGroupRepository groupRepository,
            ICategoryRepository categoryRepository,
            IPaymentRepository paymentRepository,
            IAttendanceRepository attendanceRepository,
            ILessonRepository lessonRepository,
            IExamResultRepository examResultRepository)
        {
            InitializeComponent();

            _studentRepository = studentRepository;
            _instructorRepository = instructorRepository;
            _groupRepository = groupRepository;
            _categoryRepository = categoryRepository;
            _paymentRepository = paymentRepository;
            _attendanceRepository = attendanceRepository;
            _lessonRepository = lessonRepository;
            _examResultRepository = examResultRepository;

            SetupMainForm();
        }

        private void SetupMainForm()
        {
            this.Text = "Автошкола - Учет посещаемости и обучения";
            this.Width = 800;
            this.Height = 550;
            this.StartPosition = FormStartPosition.CenterScreen;

            MenuStrip menuStrip = new MenuStrip();

            // Меню "Справочники"
            ToolStripMenuItem dictionariesMenu = new ToolStripMenuItem("Справочники");

            ToolStripMenuItem studentsItem = new ToolStripMenuItem("Студенты");
            studentsItem.Click += (s, e) => new StudentsForm(_studentRepository).ShowDialog();

            ToolStripMenuItem instructorsItem = new ToolStripMenuItem("Преподаватели");
            instructorsItem.Click += (s, e) => new InstructorsForm(_instructorRepository).ShowDialog();

            ToolStripMenuItem groupsItem = new ToolStripMenuItem("Группы");
            groupsItem.Click += (s, e) => new GroupsForm(_groupRepository).ShowDialog();

            ToolStripMenuItem categoriesItem = new ToolStripMenuItem("Категории");
            categoriesItem.Click += (s, e) => new CategoriesForm(_categoryRepository).ShowDialog();

            dictionariesMenu.DropDownItems.Add(studentsItem);
            dictionariesMenu.DropDownItems.Add(instructorsItem);
            dictionariesMenu.DropDownItems.Add(groupsItem);
            dictionariesMenu.DropDownItems.Add(categoriesItem);

            // Меню "Журналы" (операции)
            ToolStripMenuItem journalsMenu = new ToolStripMenuItem("Журналы");

            ToolStripMenuItem paymentsItem = new ToolStripMenuItem("Оплаты");
            paymentsItem.Click += (s, e) => new PaymentsForm(_paymentRepository).ShowDialog();

            ToolStripMenuItem attendanceItem = new ToolStripMenuItem("Посещаемость");
            attendanceItem.Click += (s, e) => new AttendanceForm(_attendanceRepository).ShowDialog();

            ToolStripMenuItem lessonsItem = new ToolStripMenuItem("Занятия");
            lessonsItem.Click += (s, e) => new LessonsForm(_lessonRepository).ShowDialog();

            ToolStripMenuItem examsItem = new ToolStripMenuItem("Результаты экзаменов");
            examsItem.Click += (s, e) => new ExamResultsForm(_examResultRepository).ShowDialog();

            journalsMenu.DropDownItems.Add(paymentsItem);
            journalsMenu.DropDownItems.Add(attendanceItem);
            journalsMenu.DropDownItems.Add(lessonsItem);
            journalsMenu.DropDownItems.Add(examsItem);

            // Меню "Отчеты"
            ToolStripMenuItem reportsMenu = new ToolStripMenuItem("Отчеты");
            ToolStripMenuItem emptyReportItem = new ToolStripMenuItem("Сформировать отчет...");
            emptyReportItem.Click += (s, e) => MessageBox.Show("Модуль отчетов находится в разработке.", "Отчеты", MessageBoxButtons.OK, MessageBoxIcon.Information);
            reportsMenu.DropDownItems.Add(emptyReportItem);

            // Добавляем все пункты в главное меню окна
            menuStrip.Items.Add(dictionariesMenu);
            menuStrip.Items.Add(journalsMenu);
            menuStrip.Items.Add(reportsMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }
    }
}