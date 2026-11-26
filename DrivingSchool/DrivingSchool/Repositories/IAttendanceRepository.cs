using System;
using System.Collections.Generic;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories
{
    public interface IAttendanceRepository
    {
        IEnumerable<Attendance> ReadAttendances(DateTime? dateFrom = null, DateTime? dateTo = null, int? studentId = null, int? lessonId = null);
        void CreateAttendance(Attendance attendance);
        void DeleteAttendance(int id);
    }
}