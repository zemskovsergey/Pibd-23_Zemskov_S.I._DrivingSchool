using System;
using System.Collections.Generic;
using Autofac.Features.OwnedInstances;
using DrivingSchool.Data;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public sealed class StudentRepository : EfRepository<Student>, IStudentRepository
    {
        public StudentRepository(Func<Owned<DataContext>> openContext, Serilog.ILogger logger)
            : base(openContext, logger, nameof(Student.StudentId), x => x.StudentId)
        {
        }

        public IEnumerable<Student> ReadStudents() => ReadAll(nameof(ReadStudents));

        public Student ReadStudentById(int id) => ReadById(nameof(ReadStudentById), id);

        public void CreateStudent(Student student) => Create(nameof(CreateStudent), student);

        public void UpdateStudent(Student student) =>
            Update(nameof(UpdateStudent), student, (context, existing, values) =>
            {
                var entry = context.Entry(existing);
                entry.Property(x => x.FullName).CurrentValue = values.FullName;
                entry.Property(x => x.Phone).CurrentValue = values.Phone;
                entry.Property(x => x.GroupId).CurrentValue = values.GroupId;
                entry.Property(x => x.TuitionFee).CurrentValue = values.TuitionFee;
            });

        public void DeleteStudent(int id) => Delete(nameof(DeleteStudent), id);
    }
}
