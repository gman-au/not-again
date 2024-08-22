using System;
using System.Threading.Tasks;
using Not.Again.Domain;
using Not.Again.Interfaces;

namespace Not.Again.Database
{
    public class TestRecordPutter : ITestRecordPutter
    {
        private readonly NotAgainDbContext _context;
        private readonly ITestRecordGetter _testRecordGetter;

        public TestRecordPutter(
            NotAgainDbContext context,
            ITestRecordGetter testRecordGetter
        )
        {
            _context = context;
            _testRecordGetter = testRecordGetter;
        }

        public async Task<TestRecord> AddOrUpdateTestRecordAsync(
            Guid testAssemblyId,
            string className,
            string fullName,
            string methodName,
            string testName,
            long hash,
            string delimitedTestArguments
        )
        {
            var testRecord = new TestRecord
            {
                TestAssemblyId = testAssemblyId,
                ClassName = className,
                FullName = fullName,
                MethodName = methodName,
                TestName = testName,
                DelimitedTestArguments = delimitedTestArguments,
                LastHash = hash
            };

            var dbTestRecord =
                await
                    _testRecordGetter
                        .GetAsync(
                            testAssemblyId,
                            testRecord,
                            true
                        );

            if (dbTestRecord == null)
            {
                dbTestRecord =
                    (await
                        _context
                            .TestRecord
                            .AddAsync(testRecord)
                    )
                    .Entity;
            }
            else
            {
                dbTestRecord.LastHash = testRecord.LastHash;
            }

            await
                _context
                    .SaveChangesAsync();

            return dbTestRecord;
        }
    }
}