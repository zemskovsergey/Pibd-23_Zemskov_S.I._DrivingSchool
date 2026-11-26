using System.Collections.Generic;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories
{
    public interface IInstructorRepository
    {
        IEnumerable<Instructor> ReadInstructors();
        Instructor ReadInstructorById(int id);
        void CreateInstructor(Instructor instructor);
        void DeleteInstructor(int id);
    }
}