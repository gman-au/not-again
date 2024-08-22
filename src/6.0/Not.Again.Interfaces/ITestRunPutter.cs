using System;
using System.Threading.Tasks;
using Not.Again.Domain;

namespace Not.Again.Interfaces
{
    public interface ITestRunPutter
    {
        Task<TestRun> AddTestResultAsync(
            Guid testRecordId,
            DateTime runDate,
            int result,
            long duration
        );
    }
}