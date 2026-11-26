
namespace DrivingSchool.Entities
{
    public class Instructor
    {
        public int InstructorId { get; private set; }

        public string FullName { get; private set; } = string.Empty;

        public string Phone { get; private set; } = string.Empty;

        public static Instructor CreateEntity(
            int instructorId,
            string fullName,
            string phone)
        {
            return new Instructor
            {
                InstructorId = instructorId,
                FullName = fullName ?? string.Empty,
                Phone = phone ?? string.Empty
            };
        }
    }
}
