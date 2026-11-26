using System;
using DrivingSchool.Entities.Enums;

namespace DrivingSchool.Entities
{
    public class Group
    {
        public int GroupId { get; private set; }

        public string GroupName { get; private set; } = string.Empty;

        public int CategoryId { get; private set; }

        public int InstructorId { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public TrainingDaysEnum TrainingDays { get; private set; }

        public static Group CreateEntity(
            int groupId,
            string groupName,
            int categoryId,
            int instructorId,
            DateTime startDate,
            DateTime endDate,
            TrainingDaysEnum trainingDays)
        {
            return new Group
            {
                GroupId = groupId,
                GroupName = groupName ?? string.Empty,
                CategoryId = categoryId,
                InstructorId = instructorId,
                StartDate = startDate,
                EndDate = endDate,
                TrainingDays = trainingDays
            };
        }
    }
}