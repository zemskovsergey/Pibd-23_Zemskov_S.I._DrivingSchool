using System;
using System.Collections.Generic;
using Autofac.Features.OwnedInstances;
using DrivingSchool.Data;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public sealed class GroupRepository : EfRepository<Group>, IGroupRepository
    {
        public GroupRepository(Func<Owned<DataContext>> openContext, Serilog.ILogger logger)
            : base(openContext, logger, nameof(Group.GroupId), x => x.GroupId)
        {
        }

        public IEnumerable<Group> ReadGroups() => ReadAll(nameof(ReadGroups));

        public Group ReadGroupById(int id) => ReadById(nameof(ReadGroupById), id);

        public void CreateGroup(Group group) => Create(nameof(CreateGroup), group);

        public void UpdateGroup(Group group) =>
            Update(nameof(UpdateGroup), group, (context, existing, values) =>
            {
                var entry = context.Entry(existing);
                entry.Property(x => x.GroupName).CurrentValue = values.GroupName;
                entry.Property(x => x.CategoryId).CurrentValue = values.CategoryId;
                entry.Property(x => x.InstructorId).CurrentValue = values.InstructorId;
                entry.Property(x => x.StartDate).CurrentValue = values.StartDate;
                entry.Property(x => x.EndDate).CurrentValue = values.EndDate;
                entry.Property(x => x.TrainingDays).CurrentValue = values.TrainingDays;
            });

        public void DeleteGroup(int id) => Delete(nameof(DeleteGroup), id);
    }
}
