using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Not.Again.Contracts;
using Not.Again.Domain;
using Not.Again.Interfaces;

namespace Not.Again.Database
{
    public class RunChecker : IRunChecker
    {
        private readonly IArgumentDelimiter _argumentDelimiter;
        private readonly NotAgainDbContext _context;
        private readonly ITestAssemblyGetter _testAssemblyGetter;
        private readonly ITestRecordGetter _testRecordGetter;

        public RunChecker(
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

        public async Task<bool> GetLastAsync(RunCheckRequest request)
        {
            var dbAssembly =
                await
                    _testAssemblyGetter
                        .GetAsync(
                            new TestAssembly
                            {
                                TestAssemblyName = request.TestDetails.AssemblyQualifiedName
                            }
                        );

            if (dbAssembly == null) return true;

            var delimitedArguments =
                _argumentDelimiter
                    .Perform(request.TestDetails.Arguments);

            var dbTestRecord =
                await
                    _testRecordGetter
                        .GetAsync(
                            dbAssembly.TestAssemblyId,
                            new TestRecord
                            {
                                ClassName = request.TestDetails.ClassName,
                                FullName = request.TestDetails.FullName,
                                MethodName = request.TestDetails.MethodName,
                                TestName = request.TestDetails.TestName,
                                DelimitedTestArguments = delimitedArguments,
                                LastHash = request.TestDetails.Hash
                            }
                        );

            if (dbTestRecord == null) return true;

            var lastTestRun =
                await
                    _context
                        .TestRun
                        .Where(
                            o =>
                                o.TestRecordId == dbTestRecord.TestRecordId
                        )
                        .OrderByDescending(o => o.RunDate)
                        .FirstOrDefaultAsync();

            if (lastTestRun == null) return true;

            var daysSinceTest = (DateTime.UtcNow - lastTestRun.RunDate).TotalDays;

            if (request.RerunTestsOlderThanDays.HasValue)
                return request.RerunTestsOlderThanDays.Value < daysSinceTest;

            return false;
        }
    }
}