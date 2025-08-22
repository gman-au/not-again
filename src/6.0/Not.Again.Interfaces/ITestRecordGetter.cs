using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Not.Again.Domain;

namespace Not.Again.Interfaces
{
    public interface ITestRecordGetter
    {
        Task<IEnumerable<TestRecord>> GetAsync(
            Guid? testAssemblyId = null
        );

        Task<TestRecord> GetAsync(
            Guid testAssemblyId,
            TestRecord testRecord,
            bool ignoreHash = false
        );
    }
}