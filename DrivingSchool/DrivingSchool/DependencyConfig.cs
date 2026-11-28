using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using DrivingSchool.Data;
using DrivingSchool.Forms;
using DrivingSchool.Repositories;
using DrivingSchool.Repositories.Implementations;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DrivingSchool
{
    public static class DependencyConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.Register(_ => CreateLogger()).As<ILogger>().SingleInstance();
            builder.RegisterType<DataContext>().InstancePerDependency();
            builder.RegisterType<StudentRepository>().As<IStudentRepository>();
            builder.RegisterType<InstructorRepository>().As<IInstructorRepository>();
            builder.RegisterType<GroupRepository>().As<IGroupRepository>();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>();
            builder.RegisterType<PaymentRepository>().As<IPaymentRepository>();
            builder.RegisterType<ExamResultRepository>().As<IExamResultRepository>();
            builder.RegisterType<AttendanceRepository>().As<IAttendanceRepository>();
            builder.RegisterType<LessonRepository>().As<ILessonRepository>();

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

        private static ILogger CreateLogger()
        {
            const string logPathKey = "Serilog:WriteTo:File:Args:path";
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false);
            using var settings = (ConfigurationRoot)configuration.Build();
            var logPath = Path.GetFullPath(settings[logPathKey] ?? "Logs/driving-school-.log",
                AppContext.BaseDirectory);
            configuration.AddInMemoryCollection(new Dictionary<string, string> { [logPathKey] = logPath });
            using var normalized = (ConfigurationRoot)configuration.Build();
            return new LoggerConfiguration().ReadFrom.Configuration(normalized).CreateLogger();
        }
    }
}
