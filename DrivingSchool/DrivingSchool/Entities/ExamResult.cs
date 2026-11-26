
using System;

namespace DrivingSchool.Entities
{
    public class ExamResult
    {
        public int ResultId { get; private set; }

        public int StudentId { get; private set; }

        public string TheoryScore { get; private set; } = string.Empty;

        public string DrivingScore { get; private set; } = string.Empty;

        public DateTime ExamDate { get; private set; }

        public static ExamResult CreateEntity(
            int resultId,
            int studentId,
            string theoryScore,
            string drivingScore,
            DateTime examDate)
        {
            return new ExamResult
            {
                ResultId = resultId,
                StudentId = studentId,
                TheoryScore = theoryScore ?? string.Empty,
                DrivingScore = drivingScore ?? string.Empty,
                ExamDate = examDate
            };
        }
    }
}
