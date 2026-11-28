
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrivingSchool.Entities
{
    public class ExamResult
    {
        [Key]
        public int ResultId { get; private set; }

        public int StudentId { get; private set; }

        public string TheoryScore { get; private set; } = string.Empty;

        public string DrivingScore { get; private set; } = string.Empty;

        [Column(TypeName = "date")]
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
