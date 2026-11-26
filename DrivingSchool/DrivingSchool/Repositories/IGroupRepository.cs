using System.Collections.Generic;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories
{
    public interface IGroupRepository
    {
        IEnumerable<Group> ReadGroups();
        Group ReadGroupById(int id);
        void CreateGroup(Group group);
        void DeleteGroup(int id);
    }
}