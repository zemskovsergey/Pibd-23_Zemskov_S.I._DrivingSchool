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
            studentsItem.Click += (s, e) => ShowChild(() => new StudentsForm(_studentRepository, _groupRepository));

            ToolStripMenuItem instructorsItem = new ToolStripMenuItem("Инструкторы");
            instructorsItem.Click += (s, e) => ShowChild(() => new InstructorsForm(_instructorRepository));

            ToolStripMenuItem groupsItem = new ToolStripMenuItem("Группы");
            groupsItem.Click += (s, e) => ShowChild(() => new GroupsForm(_groupRepository, _categoryRepository, _instructorRepository));

            ToolStripMenuItem categoriesItem = new ToolStripMenuItem("Категории");
            categoriesItem.Click += (s, e) => ShowChild(() => new CategoriesForm(_categoryRepository));

            dictionariesMenu.DropDownItems.Add(studentsItem);
            dictionariesMenu.DropDownItems.Add(instructorsItem);
            dictionariesMenu.DropDownItems.Add(groupsItem);
            dictionariesMenu.DropDownItems.Add(categoriesItem);

            ToolStripMenuItem journalsMenu = new ToolStripMenuItem("Операции");

            ToolStripMenuItem paymentsItem = new ToolStripMenuItem("Оплаты");
            paymentsItem.Click += (s, e) => ShowChild(() => new PaymentsForm(_paymentRepository, _studentRepository));

            ToolStripMenuItem attendanceItem = new ToolStripMenuItem("Посещаемость");
            attendanceItem.Click += (s, e) => ShowChild(() => new AttendanceForm(_attendanceRepository, _studentRepository, _lessonRepository, _groupRepository));

            ToolStripMenuItem lessonsItem = new ToolStripMenuItem("Занятия");
            lessonsItem.Click += (s, e) => ShowChild(() => new LessonsForm(_lessonRepository, _groupRepository, _instructorRepository));

            ToolStripMenuItem examsItem = new ToolStripMenuItem("Результаты экзаменов");
            examsItem.Click += (s, e) => ShowChild(() => new ExamResultsForm(_examResultRepository, _studentRepository));

            journalsMenu.DropDownItems.Add(paymentsItem);
            journalsMenu.DropDownItems.Add(attendanceItem);
            journalsMenu.DropDownItems.Add(lessonsItem);
            journalsMenu.DropDownItems.Add(examsItem);

            ToolStripMenuItem reportsMenu = new ToolStripMenuItem("Отчеты");

            menuStrip.Items.Add(dictionariesMenu);
            menuStrip.Items.Add(journalsMenu);
            menuStrip.Items.Add(reportsMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void ShowChild(Func<Form> createForm)
        {
            FormHelpers.Run(this, () =>
            {
                using var child = createForm();
                child.ShowDialog(this);
            });
        }
    }
}
