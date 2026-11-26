using System.Collections.Generic;
using System.Linq;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public class InstructorRepositoryMock : IInstructorRepository
    {
        private readonly List<Instructor> _instructors = new List<Instructor>();

        public IEnumerable<Instructor> ReadInstructors() => _instructors;

        public Instructor ReadInstructorById(int id) => _instructors.FirstOrDefault(i => i.InstructorId == id);

        public void CreateInstructor(Instructor instructor)
        {
            _instructors.Add(instructor);
        }

        public void UpdateInstructor(Instructor instructor)
        {
            var existing = ReadInstructorById(instructor.InstructorId);
            if (existing != null)
            {
                _instructors.Remove(existing);
                _instructors.Add(instructor);
            }
        }

        public void DeleteInstructor(int id)
        {
            var instructor = ReadInstructorById(id);
            if (instructor != null)
            {
                _instructors.Remove(instructor);
            }
        }
    }
}