using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.OwnedInstances;
using DrivingSchool.Data;
using DrivingSchool.Entities;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchool.Repositories.Implementations
{
    public sealed class AttendanceRepository : EfRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(Func<Owned<DataContext>> openContext, Serilog.ILogger logger)
            : base(openContext, logger, nameof(Attendance.AttendanceId), x => x.AttendanceId)
        {
        }

        public IEnumerable<Attendance> ReadAttendances(DateTime? dateFrom = null, DateTime? dateTo = null,
            int? studentId = null, int? lessonId = null) => Run(nameof(ReadAttendances), null, context =>
        {
            var from = dateFrom?.Date;
            var to = dateTo?.Date;
            if (from > to)
                throw new ArgumentException("Дата начала не должна быть позже даты окончания.");

            var query = context.Attendance.AsNoTracking();
            if (studentId is int student)
                query = query.Where(x => x.StudentId == student);
            if (lessonId is int lesson)
                query = query.Where(x => x.LessonId == lesson);
            if (from is DateTime first)
                query = query.Where(x => context.Lessons.Any(l => l.LessonId == x.LessonId && l.LessonDate >= first));
            if (to is DateTime last)
                query = query.Where(x => context.Lessons.Any(l => l.LessonId == x.LessonId && l.LessonDate <= last));

            var result = OrderById(query).ToList();
            Logger.Information("{Operation} with student ID {StudentId} and lesson ID {LessonId} returned {Count} records",
                nameof(ReadAttendances), studentId, lessonId, result.Count);
            return result;
        });

        public void CreateAttendance(Attendance attendance) =>
            Run(nameof(CreateAttendance), attendance?.AttendanceId, context =>
            {
                ArgumentNullException.ThrowIfNull(attendance);
                Logger.Information("{Operation} started for student ID {StudentId}, lesson ID {LessonId}, present: {IsPresent}",
                    nameof(CreateAttendance), attendance.StudentId, attendance.LessonId, attendance.IsPresent);

                using var transaction = context.Database.BeginTransaction();
                int count;
                try
                {
                    var student = context.Students.AsNoTracking()
                        .SingleOrDefault(x => x.StudentId == attendance.StudentId)
                        ?? throw new InvalidOperationException("Учащийся не найден.");
                    var lesson = context.Lessons.AsNoTracking()
                        .SingleOrDefault(x => x.LessonId == attendance.LessonId)
                        ?? throw new InvalidOperationException("Занятие не найдено.");
                    if (student.GroupId != lesson.GroupId)
                        throw new InvalidOperationException("Учащийся не входит в группу выбранного занятия.");

                    if (context.Attendance.Any(x => x.StudentId == attendance.StudentId && x.LessonId == attendance.LessonId))
                        throw new InvalidOperationException("Посещаемость для этого учащегося на выбранном занятии уже отмечена.");

                    context.Attendance.Add(attendance);
                    count = context.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

                LogSaved(nameof(CreateAttendance), attendance.AttendanceId, count, "Committed");
                return count;
            });

        public void DeleteAttendance(int id) => Delete(nameof(DeleteAttendance), id);
    }
}
