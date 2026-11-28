using System;
using System.Collections.Generic;
using Autofac.Features.OwnedInstances;
using DrivingSchool.Data;
using DrivingSchool.Entities;

namespace DrivingSchool.Repositories.Implementations
{
    public sealed class ExamResultRepository : EfRepository<ExamResult>, IExamResultRepository
    {
        public ExamResultRepository(Func<Owned<DataContext>> openContext, Serilog.ILogger logger)
            : base(openContext, logger, nameof(ExamResult.ResultId), x => x.ResultId)
        {
        }

        public IEnumerable<ExamResult> ReadExamResults() => ReadAll(nameof(ReadExamResults));

        public void CreateExamResult(ExamResult examResult) => Create(nameof(CreateExamResult), examResult);

        public void DeleteExamResult(int id) => Delete(nameof(DeleteExamResult), id);
    }
}
