using System;
using System.Threading.Tasks;
using Not.Again.Domain;

namespace Not.Again.Interfaces
{
    public interface ITestRecordGetter
    {
        Task<TestRecord> GetAsync(
            Guid testAssemblyId,
            TestRecord testRecord,
            bool ignoreHash = false
        );
    }
}