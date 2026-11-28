using System.Collections.Generic;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories
{
    public interface IStudentRepository
    {
        IEnumerable<Student> ReadStudents();
        Student ReadStudentById(int id);
        void CreateStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
    }
}