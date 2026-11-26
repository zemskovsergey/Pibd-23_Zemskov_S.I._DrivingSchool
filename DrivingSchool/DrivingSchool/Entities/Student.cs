
namespace DrivingSchool.Entities
{
    public class Student
    {
        public int StudentId { get; private set; }

        public string FullName { get; private set; } = string.Empty;

        public string Phone { get; private set; } = string.Empty;

        public int GroupId { get; private set; }

        public decimal TuitionFee { get; private set; }

        public static Student CreateEntity(
            int studentId,
            string fullName,
            string phone,
            int groupId,
            decimal tuitionFee)
        {
            return new Student
            {
                StudentId = studentId,
                FullName = fullName ?? string.Empty,
                Phone = phone ?? string.Empty,
                GroupId = groupId,
                TuitionFee = tuitionFee
            };
        }
    }
}
