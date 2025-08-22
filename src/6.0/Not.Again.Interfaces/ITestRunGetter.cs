using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Not.Again.Domain;

namespace Not.Again.Interfaces
{
    public interface ITestRunGetter
    {
        Task<TestRun> GetLastRunAsync(Guid testRecordId);

        Task<IEnumerable<TestRun>> GetTestRunsAsync(
            Guid? assemblyId,
            Guid? testRecordId
        );
    }
}