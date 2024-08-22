using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Not.Again.Contracts;
using Not.Again.Domain;
using Not.Again.Enum;
using Not.Again.Interfaces;

namespace Not.Again.Infrastructure
{
    public class RunChecker : IRunChecker
    {
        private readonly IArgumentDelimiter _argumentDelimiter;
        private readonly IMessageFormatter _messageFormatter;
        private readonly ITestAssemblyGetter _testAssemblyGetter;
        private readonly ITestRecordGetter _testRecordGetter;
        private readonly ITestRunGetter _testRunGetter;
        private readonly ILogger<RunChecker> _logger;

        public RunChecker(
            ITestAssemblyGetter testAssemblyGetter,
            ITestRecordGetter testRecordGetter,
            ITestRunGetter testRunGetter,
            IArgumentDelimiter argumentDelimiter,
            IMessageFormatter messageFormatter,
            ILogger<RunChecker> logger
        )
        {
            _testAssemblyGetter = testAssemblyGetter;
            _testRecordGetter = testRecordGetter;
            _testRunGetter = testRunGetter;
            _argumentDelimiter = argumentDelimiter;
            _messageFormatter = messageFormatter;
            _logger = logger;
        }

        public async Task<DiagnosticResponse> GetLastAsync(RunCheckRequest request)
        {
            var result = new DiagnosticResponse
            {
                IgnoreThisTest = false,
                Message = null
            };

            try
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

                if (dbAssembly == null)
                {
                    result.Message =
                        _messageFormatter
                            .EncapsulateNotAgainMessage(
                                string.Format(
                                    StandardMessages.AssemblyNotFound,
                                    request.TestDetails.AssemblyQualifiedName,
                                    request.TestDetails.FullName
                                )
                            );

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
                            .EncapsulateNotAgainMessage(
                                string.Format(
                                    StandardMessages.RecordNotFound,
                                    request.TestDetails.FullName
                                )
                            );

                    return result;
                }

                var lastTestRun =
                    await
                        _testRunGetter
                            .GetLastRunAsync(dbTestRecord.TestRecordId);

                if (lastTestRun == null)
                {
                    result.Message =
                        _messageFormatter
                            .EncapsulateNotAgainMessage(
                                string.Format(
                                    StandardMessages.RunNotFound,
                                    request.TestDetails.FullName
                                )
                            );

                    return result;
                }

                if (lastTestRun.Result != (int)TestResultEnum.Passed)
                {
                    result.Message =
                        _messageFormatter
                            .EncapsulateNotAgainMessage(
                                string
                                    .Format(
                                        StandardMessages
                                            .LastRunFailed,
                                        request.TestDetails.FullName
                                    )
                            );

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
                                .EncapsulateNotAgainMessage(
                                    string
                                        .Format(
                                            StandardMessages
                                                .NewerRunFound,
                                            request.TestDetails.FullName,
                                            request.RerunTestsOlderThanDays.Value
                                        )
                                );

                        return result;
                    }

                    result.Message =
                        _messageFormatter
                            .EncapsulateNotAgainMessage(
                                string
                                    .Format(
                                        StandardMessages
                                            .OnlyOlderRunFound,
                                        request.TestDetails.FullName,
                                        request.RerunTestsOlderThanDays.Value
                                    )
                            );

                    return result;
                }

                result.Message =
                    _messageFormatter
                        .EncapsulateNotAgainMessage(
                            string
                                .Format(
                                    StandardMessages
                                        .NoIntervalSpecifiedForFoundRun,
                                    request.TestDetails.FullName
                                )
                        );
            }
            catch (Exception ex)
            {
                _logger
                    .LogError(ex.Message);

                result.Message =
                    _messageFormatter
                        .EncapsulateNotAgainMessage(
                            "The NotAgain service encountered an error when processing this request; please check the server logs for more information"
                        );
            }

            return result;
        }
    }
}