using Autofac;
using DrivingSchool.Entities;
using DrivingSchool.Repositories;
using DrivingSchool.Repositories.Implementations;
using DrivingSchool.Forms;

namespace DrivingSchool
{
    public static class DependencyConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<StudentRepositoryMock>().As<IStudentRepository>().SingleInstance();
            builder.RegisterType<InstructorRepositoryMock>().As<IInstructorRepository>().SingleInstance();
            builder.RegisterType<GroupRepositoryMock>().As<IGroupRepository>().SingleInstance();
            builder.RegisterType<CategoryRepositoryMock>().As<ICategoryRepository>().SingleInstance();
            builder.RegisterType<PaymentRepositoryMock>().As<IPaymentRepository>().SingleInstance();
            builder.RegisterType<ExamResultRepositoryMock>().As<IExamResultRepository>().SingleInstance();
            builder.RegisterType<AttendanceRepositoryMock>().As<IAttendanceRepository>().SingleInstance();
            builder.RegisterType<LessonRepositoryMock>().As<ILessonRepository>().SingleInstance();

            builder.RegisterType<MainForm>().AsSelf();
            builder.RegisterType<AttendanceForm>().AsSelf();
            builder.RegisterType<CategoriesForm>().AsSelf();
            builder.RegisterType<ExamResultsForm>().AsSelf();
            builder.RegisterType<GroupsForm>().AsSelf();
            builder.RegisterType<InstructorsForm>().AsSelf();
            builder.RegisterType<LessonsForm>().AsSelf();
            builder.RegisterType<PaymentsForm>().AsSelf();
            builder.RegisterType<StudentsForm>().AsSelf();

            return builder.Build();
        }
    }
}