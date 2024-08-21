using System;
using System.Threading.Tasks;
using Not.Again.Domain;

namespace Not.Again.Interfaces
{
    public interface ITestRunGetter
    {
        Task<TestRun> GetLastRunAsync(Guid testRecordId);
    }
}