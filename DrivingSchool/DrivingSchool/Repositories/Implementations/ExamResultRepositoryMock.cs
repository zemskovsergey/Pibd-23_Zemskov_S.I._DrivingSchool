using System.Collections.Generic;
using System.Linq;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public class ExamResultRepositoryMock : IExamResultRepository
    {
        private readonly List<ExamResult> _examResults = new List<ExamResult>();

        public IEnumerable<ExamResult> ReadExamResults() => _examResults;

        public void CreateExamResult(ExamResult examResult)
        {
            _examResults.Add(examResult);
        }

        public void DeleteExamResult(int id)
        {
            var item = _examResults.FirstOrDefault(e => e.ResultId == id);
            if (item != null)
            {
                _examResults.Remove(item);
            }
        }
    }
}