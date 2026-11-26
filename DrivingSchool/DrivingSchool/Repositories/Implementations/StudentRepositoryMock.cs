using System.Collections.Generic;
using System.Linq;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public class StudentRepositoryMock : IStudentRepository
    {
        private readonly List<Student> _students = new List<Student>();

        public IEnumerable<Student> ReadStudents() => _students;

        public Student ReadStudentById(int id) => _students.FirstOrDefault(s => s.StudentId == id);

        public void CreateStudent(Student student)
        {
            _students.Add(student);
        }

        public void UpdateStudent(Student student)
        {
            var existing = ReadStudentById(student.StudentId);
            if (existing != null)
            {
                _students.Remove(existing);
                _students.Add(student);
            }
        }

        public void DeleteStudent(int id)
        {
            var student = ReadStudentById(id);
            if (student != null)
            {
                _students.Remove(student);
            }
        }
    }
}