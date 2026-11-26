using System;
using System.Collections.Generic;
using System.Linq;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public class AttendanceRepositoryMock : IAttendanceRepository
    {
        private readonly List<Attendance> _attendances = new List<Attendance>();

        public IEnumerable<Attendance> ReadAttendances(DateTime? dateFrom = null, DateTime? dateTo = null, int? studentId = null, int? lessonId = null)
        {
            var query = _attendances.AsEnumerable();

            if (studentId.HasValue)
            {
                query = query.Where(x => x.StudentId == studentId.Value);
            }
            if (lessonId.HasValue)
            {
                query = query.Where(x => x.LessonId == lessonId.Value);
            }

            return query.ToList();
        }

        public void CreateAttendance(Attendance attendance)
        {
            _attendances.Add(attendance);
        }

        public void DeleteAttendance(int id)
        {
            var item = _attendances.FirstOrDefault(x => x.AttendanceId == id);
            if (item != null)
            {
                _attendances.Remove(item);
            }
        }
    }
}