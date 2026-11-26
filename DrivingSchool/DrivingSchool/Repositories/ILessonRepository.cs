using System.Collections.Generic;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories
{
    public interface ILessonRepository
    {
        IEnumerable<Lesson> ReadLessons();
        Lesson ReadLessonById(int id);
        void CreateLesson(Lesson lesson);
        void DeleteLesson(int id);
    }
}