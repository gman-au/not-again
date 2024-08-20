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
        private readonly IMessageFormatter _messageFormatter;

        public RunChecker(
            ITestAssemblyGetter testAssemblyGetter,
            ITestRecordGetter testRecordGetter,
            IArgumentDelimiter argumentDelimiter,
            NotAgainDbContext context, 
            IMessageFormatter messageFormatter
        )
        {
            _testAssemblyGetter = testAssemblyGetter;
            _testRecordGetter = testRecordGetter;
            _argumentDelimiter = argumentDelimiter;
            _context = context;
            _messageFormatter = messageFormatter;
        }

        public async Task<DiagnosticResponse> GetLastAsync(RunCheckRequest request)
        {
            var result = new DiagnosticResponse
            {
                IgnoreThisTest = false,
                Message = null
            };
            
            var dbAssembly =
                await
                    _testAssemblyGetter
                        .GetAsync(
                            new TestAssembly
                            {
                                TestAssemblyName = request.TestDetails.AssemblyQualifiedName
                            }
                        );

            if (dbAssembly == null)
            {
                result.Message = 
                    _messageFormatter
                        .EncapsulateNotAgainMessage($"No prior record of this test assembly [{request.TestDetails.AssemblyQualifiedName}], the test [{request.TestDetails.FullName}] should NOT be ignored");
                
                return result;
            }

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

            if (dbTestRecord == null)
            {
                result.Message = 
                    _messageFormatter
                        .EncapsulateNotAgainMessage($"The test [{request.TestDetails.FullName}] is either new or has been modified since last run - it should NOT be ignored");
                
                return result;
            }

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

            if (lastTestRun == null)
            {
                result.Message = 
                    _messageFormatter
                        .EncapsulateNotAgainMessage($"No prior test run found for this test record [{request.TestDetails.FullName}] - it should NOT be ignored");
                
                return result;
            }

            var daysSinceTest = (DateTime.UtcNow - lastTestRun.RunDate).TotalDays;

            if (request.RerunTestsOlderThanDays.HasValue)
            {
                result.IgnoreThisTest = request.RerunTestsOlderThanDays.Value > daysSinceTest;

                if (result.IgnoreThisTest)
                {
                    result.Message = 
                        _messageFormatter
                            .EncapsulateNotAgainMessage($"Last run for test [{request.TestDetails.FullName}] did not exceed the specified interval of {request.RerunTestsOlderThanDays.Value} days - it should be ignored");
                    
                    return result;
                }
                
                result.Message = 
                    _messageFormatter
                        .EncapsulateNotAgainMessage($"Last run for test [{request.TestDetails.FullName}] exceeded the specified interval of {request.RerunTestsOlderThanDays.Value} days - it should NOT be ignored");
                
                return result;
            }

            return result;
        }
    }
}