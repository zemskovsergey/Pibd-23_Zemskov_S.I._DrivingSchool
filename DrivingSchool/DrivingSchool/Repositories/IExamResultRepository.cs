using System.Collections.Generic;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories
{
    public interface IExamResultRepository
    {
        IEnumerable<ExamResult> ReadExamResults();
        void CreateExamResult(ExamResult examResult);
        void DeleteExamResult(int id);
    }
}