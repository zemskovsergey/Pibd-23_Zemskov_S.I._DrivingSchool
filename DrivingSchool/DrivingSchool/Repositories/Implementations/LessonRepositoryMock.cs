using System.Collections.Generic;
using System.Linq;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public class LessonRepositoryMock : ILessonRepository
    {
        private readonly List<Lesson> _lessons = new List<Lesson>();

        public IEnumerable<Lesson> ReadLessons() => _lessons;

        public Lesson ReadLessonById(int id) => _lessons.FirstOrDefault(l => l.LessonId == id);

        public void CreateLesson(Lesson lesson)
        {
            _lessons.Add(lesson);
        }

        public void UpdateLesson(Lesson lesson)
        {
            var existing = ReadLessonById(lesson.LessonId);
            if (existing != null)
            {
                _lessons.Remove(existing);
                _lessons.Add(lesson);
            }
        }

        public void DeleteLesson(int id)
        {
            var lesson = ReadLessonById(id);
            if (lesson != null)
            {
                _lessons.Remove(lesson);
            }
        }
    }
}