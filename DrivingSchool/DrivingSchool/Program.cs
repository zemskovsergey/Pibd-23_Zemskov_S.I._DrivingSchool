using System;
using System.Windows.Forms;
using Autofac;

namespace DrivingSchool
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Инициализируем и настраиваем контейнер Autofac
            var container = DependencyConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var mainForm = scope.Resolve<MainForm>();
                Application.Run(mainForm);
            }
        }
    }
}