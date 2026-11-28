using System;
using System.Collections.Generic;
using Autofac.Features.OwnedInstances;
using DrivingSchool.Data;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public sealed class LessonRepository : EfRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(Func<Owned<DataContext>> openContext, Serilog.ILogger logger)
            : base(openContext, logger, nameof(Lesson.LessonId), x => x.LessonId)
        {
        }

        public IEnumerable<Lesson> ReadLessons() => ReadAll(nameof(ReadLessons));

        public Lesson ReadLessonById(int id) => ReadById(nameof(ReadLessonById), id);

        public void CreateLesson(Lesson lesson) => Create(nameof(CreateLesson), lesson);

        public void DeleteLesson(int id) => Delete(nameof(DeleteLesson), id);
    }
}
