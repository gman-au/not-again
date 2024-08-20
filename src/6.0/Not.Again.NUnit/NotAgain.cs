using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Not.Again.Infrastructure;
using Not.Again.NUnit.Extensions;
using NUnit.Framework;

namespace Not.Again.NUnit
{
    public static class NotAgain
    {
        private static readonly string NotAgainUrl = Environment.GetEnvironmentVariable(StandardConstants.BaseUrlVariableName);
        private static readonly Stopwatch Stopwatch = new();

        private static bool _submitResult;

        public static async Task SetupAsync()
        {
            _submitResult = true;

            bool alreadyReported;

            var context =
                TestContext
                    .CurrentContext;

            var runCheckRequest =
                context
                    .ToRunCheckRequest();

            if (!string.IsNullOrEmpty(NotAgainUrl))
            {
                alreadyReported =
                    await
                        ApiAdapter
                            .RunCheckAsync(
                                runCheckRequest,
                                NotAgainUrl,
                                TestContext.WriteLine
                            );
            }
            else
            {
                _submitResult = false;
                
                TestContext
                    .WriteLine(StandardMessages.NoUrlEnvVariableSuppliedMessage);

                return;
            }

            if (!alreadyReported)
            {
                TestContext
                    .WriteLine(StandardMessages.RunningThisTestMessage);

                Stopwatch
                    .Start();
            }
            else
            {
                _submitResult = false;
                
                TestContext
                    .WriteLine(StandardMessages.IgnoringThisTestMessage);
                Assert
                    .Ignore(StandardMessages.IgnoringThisTestMessage);
            }
        }

        public static async Task TearDownAsync()
        {
            Stopwatch
                .Stop();

            if (!_submitResult) return;

            if (!string.IsNullOrEmpty(NotAgainUrl))
            {
                var context =
                    TestContext
                        .CurrentContext;

                var submitResultRequest =
                    context
                        .ToSubmitResultRequest(Stopwatch.ElapsedMilliseconds);

                await
                    ApiAdapter
                        .SubmitResultAsync(
                            submitResultRequest,
                            NotAgainUrl,
                            TestContext.WriteLine
                        );
            }
            else
            {
                TestContext
                    .WriteLine(StandardMessages.NoUrlEnvVariableSuppliedMessage);
            }
        }
    }
}