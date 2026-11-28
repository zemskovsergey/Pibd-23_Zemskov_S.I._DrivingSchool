using System;
using System.IO;
using DrivingSchool.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DrivingSchool.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Instructor> Instructors => Set<Instructor>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<Attendance> Attendance => Set<Attendance>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<ExamResult> ExamResults => Set<ExamResult>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            var connectionString = Environment.GetEnvironmentVariable("DRIVINGSCHOOL_CONNECTION_STRING");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                var profile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var passwordFile = Path.Combine(profile, "DrivingSchool", "pgpass.conf");
                if (!File.Exists(passwordFile) && File.Exists(passwordFile + ".txt"))
                    passwordFile += ".txt";

                if (!File.Exists(passwordFile))
                    throw new InvalidOperationException(
                        "Не настроено подключение к PostgreSQL. Укажите DRIVINGSCHOOL_CONNECTION_STRING " +
                        "или сохраните pgpass.conf в папке %APPDATA%\\DrivingSchool.");

                connectionString = new NpgsqlConnectionStringBuilder
                {
                    Host = "127.0.0.1",
                    Port = 5432,
                    Database = "driving_school",
                    Username = "postgres",
                    Passfile = passwordFile
                }.ConnectionString;
            }

            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasOne<Category>().WithMany()
                .HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Group>()
                .HasOne<Instructor>().WithMany()
                .HasForeignKey(x => x.InstructorId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne<Group>().WithMany()
                .HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Lesson>()
                .HasOne<Group>().WithMany()
                .HasForeignKey(x => x.GroupId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Lesson>()
                .HasOne<Instructor>().WithMany()
                .HasForeignKey(x => x.InstructorId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne<Student>().WithMany()
                .HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExamResult>()
                .HasOne<Student>().WithMany()
                .HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attendance>()
                .HasOne<Student>().WithMany()
                .HasForeignKey(x => x.StudentId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attendance>()
                .HasOne<Lesson>().WithMany()
                .HasForeignKey(x => x.LessonId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attendance>()
                .HasIndex(x => new { x.StudentId, x.LessonId }).IsUnique();

            modelBuilder.Entity<Student>()
                .Property(x => x.TuitionFee).HasPrecision(12, 2);

            modelBuilder.Entity<Payment>()
                .Property(x => x.Amount).HasPrecision(12, 2);

            modelBuilder.Entity<Group>()
                .Property(x => x.StartDate).HasColumnType("date");

            modelBuilder.Entity<Group>()
                .Property(x => x.EndDate).HasColumnType("date");

            modelBuilder.Entity<Lesson>()
                .Property(x => x.LessonDate).HasColumnType("date");

            modelBuilder.Entity<Payment>()
                .Property(x => x.PaymentDate).HasColumnType("date");
        }
    }
}
