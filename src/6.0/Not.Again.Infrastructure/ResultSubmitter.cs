using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Not.Again.Contracts;
using Not.Again.Interfaces;

namespace Not.Again.Infrastructure
{
    public class ResultSubmitter : IResultSubmitter
    {
        private readonly IArgumentDelimiter _argumentDelimiter;
        private readonly ITestAssemblyPutter _testAssemblyPutter;
        private readonly ITestRecordPutter _testRecordPutter;
        private readonly ITestRunPutter _testRunPutter;
        private readonly ILogger<ResultSubmitter> _logger;

        public ResultSubmitter(
            ITestAssemblyPutter testAssemblyPutter,
            IArgumentDelimiter argumentDelimiter,
            ITestRunPutter testRunPutter,
            ITestRecordPutter testRecordPutter, 
            ILogger<ResultSubmitter> logger
        )
        {
            _testAssemblyPutter = testAssemblyPutter;
            _argumentDelimiter = argumentDelimiter;
            _testRunPutter = testRunPutter;
            _testRecordPutter = testRecordPutter;
            _logger = logger;
        }

        public async Task SubmitResultAsync(SubmitResultRequest request)
        {
            var dbAssembly =
                await
                    _testAssemblyPutter
                        .AddOrUpdateTestAssemblyAsync(
                            request.TestDetails.AssemblyQualifiedName,
                            request.TestResultDetails.TestRunner
                        );

            var delimitedArguments =
                _argumentDelimiter
                    .Perform(request.TestDetails.Arguments);

            var dbTestRecord =
                await
                    _testRecordPutter
                        .AddOrUpdateTestRecordAsync(
                            dbAssembly.TestAssemblyId,
                            request.TestDetails.ClassName,
                            request.TestDetails.FullName,
                            request.TestDetails.MethodName,
                            request.TestDetails.TestName,
                            request.TestDetails.Hash,
                            delimitedArguments
                        );

            await
                _testRunPutter
                    .AddTestResultAsync(
                        dbTestRecord.TestRecordId,
                        request.TestResultDetails.RunDate,
                        request.TestResultDetails.Status,
                        request.TestResultDetails.Duration
                    );
            
            _logger
                .LogInformation($"Successfully submitted test result for test [{request.TestDetails.TestName}]");
        }
    }
}