using System.Collections.Generic;
using System.Linq;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public class GroupRepositoryMock : IGroupRepository
    {
        private readonly List<Group> _groups = new List<Group>();

        public IEnumerable<Group> ReadGroups() => _groups;

        public Group ReadGroupById(int id) => _groups.FirstOrDefault(g => g.GroupId == id);

        public void CreateGroup(Group group)
        {
            _groups.Add(group);
        }

        public void UpdateGroup(Group group)
        {
            var existing = ReadGroupById(group.GroupId);
            if (existing != null)
            {
                _groups.Remove(existing);
                _groups.Add(group);
            }
        }

        public void DeleteGroup(int id)
        {
            var group = ReadGroupById(id);
            if (group != null)
            {
                _groups.Remove(group);
            }
        }
    }
}