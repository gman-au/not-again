using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Not.Again.Infrastructure;
using Not.Again.NUnit.Extensions;
using NUnit.Framework;

namespace Not.Again.NUnit
{
    public class NotAgain
    {
        private const string BaseUrlVariableName = "NOT_AGAIN_URL";
        private const string IgnoreMessage = "This test has been run previously - ignoring";

        private static readonly string NotAgainUrl = Environment.GetEnvironmentVariable(BaseUrlVariableName);

        private static bool _submitResult;
        private static readonly Stopwatch Stopwatch = new();

        public static async Task SetupAsync()
        {
            _submitResult = true;

            var alreadyReported = false;

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

            if (!alreadyReported)
            {
                TestContext
                    .WriteLine("No previous applicable test run reported, running this test...");

                Stopwatch
                    .Start();
            }
            else
            {
                _submitResult = false;
                TestContext
                    .WriteLine(IgnoreMessage);
                Assert
                    .Ignore(IgnoreMessage);
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
        }
    }
}