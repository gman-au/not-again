using System;
using System.Threading.Tasks;
using Not.Again.Contracts;
using Not.Again.Domain;
using Not.Again.Interfaces;

namespace Not.Again.Database
{
    public class ResultSubmitter : IResultSubmitter
    {
        private readonly IArgumentDelimiter _argumentDelimiter;
        private readonly NotAgainDbContext _context;
        private readonly ITestAssemblyGetter _testAssemblyGetter;
        private readonly ITestRecordGetter _testRecordGetter;
        
        public ResultSubmitter(
            ITestAssemblyGetter testAssemblyGetter,
            ITestRecordGetter testRecordGetter,
            IArgumentDelimiter argumentDelimiter,
            NotAgainDbContext context
        )
        {
            _testAssemblyGetter = testAssemblyGetter;
            _testRecordGetter = testRecordGetter;
            _argumentDelimiter = argumentDelimiter;
            _context = context;
        }
        
        public async Task SubmitResultAsync(SubmitResultRequest request)
        {
            var dbAssembly =
                await
                    AddOrUpdateTestAssemblyAsync(request.TestDetails.AssemblyQualifiedName);

            var delimitedArguments =
                _argumentDelimiter
                    .Perform(request.TestDetails.Arguments);

            var dbTestRecord =
                await
                    AddOrUpdateTestRecordAsync(
                        dbAssembly.TestAssemblyId,
                        request.TestDetails.ClassName,
                        request.TestDetails.FullName,
                        request.TestDetails.MethodName,
                        request.TestDetails.TestName,
                        request.TestDetails.Hash,
                        delimitedArguments
                    );

            await
                AddTestResultAsync(
                    dbTestRecord.TestRecordId,
                    request.TestResultDetails.RunDate,
                    request.TestResultDetails.Status,
                    request.TestResultDetails.Duration
                );
        }

        private async Task<TestAssembly> AddOrUpdateTestAssemblyAsync(string assemblyName)
        {
            try
            {
                var testAssembly = new TestAssembly
                {
                    TestAssemblyName = assemblyName
                };

                var dbTestAssembly =
                    await
                        _testAssemblyGetter
                            .GetAsync(testAssembly);

                if (dbTestAssembly == null)
                {
                    dbTestAssembly =
                        (await
                            _context
                                .TestAssembly
                                .AddAsync(testAssembly))
                        .Entity;

                    await
                        _context
                            .SaveChangesAsync();
                }

                return dbTestAssembly;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<TestRecord> AddOrUpdateTestRecordAsync(
            Guid testAssemblyId,
            string className,
            string fullName,
            string methodName,
            string testName,
            long hash,
            string delimitedTestArguments
        )
        {
            try
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
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<TestRun> AddTestResultAsync(
            Guid testRecordId,
            DateTime runDate,
            int result,
            long duration
        )
        {
            try
            {
                var testRun = new TestRun
                {
                    TestRecordId = testRecordId,
                    RunDate = runDate,
                    Result = result,
                    TotalDuration = duration
                };

                var dbTestRun =
                    (await
                        _context
                            .TestRun
                            .AddAsync(testRun))
                    .Entity;

                await
                    _context
                        .SaveChangesAsync();

                return dbTestRun;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}