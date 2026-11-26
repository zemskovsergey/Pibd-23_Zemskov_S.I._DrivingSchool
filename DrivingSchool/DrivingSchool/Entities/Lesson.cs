
using System;

namespace DrivingSchool.Entities
{
    public class Lesson
    {
        public int LessonId { get; private set; }

        public int GroupId { get; private set; }

        public int InstructorId { get; private set; }

        public DateTime LessonDate { get; private set; }

        public string LessonType { get; private set; } = string.Empty;

        public static Lesson CreateEntity(
            int lessonId,
            int groupId,
            int instructorId,
            DateTime lessonDate,
            string lessonType)
        {
            return new Lesson
            {
                LessonId = lessonId,
                GroupId = groupId,
                InstructorId = instructorId,
                LessonDate = lessonDate,
                LessonType = lessonType ?? string.Empty
            };
        }
    }
}
