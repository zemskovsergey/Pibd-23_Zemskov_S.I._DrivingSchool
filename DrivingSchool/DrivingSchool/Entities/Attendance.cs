
namespace DrivingSchool.Entities
{
    public class Attendance
    {
        public int AttendanceId { get; private set; }

        public int StudentId { get; private set; }

        public bool IsPresent { get; private set; }

        public int LessonId { get; private set; }

        public static Attendance CreateEntity(
            int attendanceId,
            int studentId,
            bool isPresent,
            int lessonId)
        {
            return new Attendance
            {
                AttendanceId = attendanceId,
                StudentId = studentId,
                IsPresent = isPresent,
                LessonId = lessonId
            };
        }
    }
}
