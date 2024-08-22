using System;
using System.Threading.Tasks;
using Not.Again.Domain;

namespace Not.Again.Interfaces
{
    public interface ITestRecordPutter
    {
        Task<TestRecord> AddOrUpdateTestRecordAsync(
            Guid testAssemblyId,
            string className,
            string fullName,
            string methodName,
            string testName,
            long hash,
            string delimitedTestArguments
        );
    }
}