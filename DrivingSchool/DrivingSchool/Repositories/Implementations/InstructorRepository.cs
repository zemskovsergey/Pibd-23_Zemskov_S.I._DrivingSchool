using System;
using System.Collections.Generic;
using Autofac.Features.OwnedInstances;
using DrivingSchool.Data;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public sealed class InstructorRepository : EfRepository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(Func<Owned<DataContext>> openContext, Serilog.ILogger logger)
            : base(openContext, logger, nameof(Instructor.InstructorId), x => x.InstructorId)
        {
        }

        public IEnumerable<Instructor> ReadInstructors() => ReadAll(nameof(ReadInstructors));

        public Instructor ReadInstructorById(int id) => ReadById(nameof(ReadInstructorById), id);

        public void CreateInstructor(Instructor instructor) => Create(nameof(CreateInstructor), instructor);

        public void UpdateInstructor(Instructor instructor) =>
            Update(nameof(UpdateInstructor), instructor, (context, existing, values) =>
            {
                var entry = context.Entry(existing);
                entry.Property(x => x.FullName).CurrentValue = values.FullName;
                entry.Property(x => x.Phone).CurrentValue = values.Phone;
            });

        public void DeleteInstructor(int id) => Delete(nameof(DeleteInstructor), id);
    }
}
