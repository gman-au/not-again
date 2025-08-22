using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Not.Again.Domain;
using Not.Again.Interfaces;

namespace Not.Again.Database
{
    public class TestRecordGetter : ITestRecordGetter
    {
        private readonly NotAgainDbContext _context;

        public TestRecordGetter(NotAgainDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TestRecord>> GetAsync(
            Guid? testAssemblyId = null
        )
        {
            var dbTestRecords =
                await
                    _context
                        .TestRecord
                        .Where(o => !testAssemblyId.HasValue || testAssemblyId.Value == o.TestAssemblyId)
                        .ToListAsync();

            return dbTestRecords;
        }

        public async Task<TestRecord> GetAsync(
            Guid testAssemblyId,
            TestRecord testRecord,
            bool ignoreHash = false
        )
        {
            var dbTestRecord =
                await
                    _context
                        .TestRecord
                        .FirstOrDefaultAsync(
                            o =>
                                o.TestAssemblyId == testAssemblyId &&
                                EF.Functions.Like(
                                    o.ClassName,
                                    testRecord.ClassName
                                ) &&
                                EF.Functions.Like(
                                    o.FullName,
                                    testRecord.FullName
                                ) &&
                                EF.Functions.Like(
                                    o.MethodName,
                                    testRecord.MethodName
                                ) &&
                                EF.Functions.Like(
                                    o.TestName,
                                    testRecord.TestName
                                ) &&
                                EF.Functions.Like(
                                    o.DelimitedTestArguments,
                                    testRecord.DelimitedTestArguments
                                ) &&
                                (ignoreHash || o.LastHash == testRecord.LastHash)
                        );

            return dbTestRecord;
        }
    }
}