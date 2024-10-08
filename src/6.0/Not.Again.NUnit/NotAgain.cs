﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Not.Again.Contracts;
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

            int
                .TryParse(
                    Environment.GetEnvironmentVariable(StandardConstants.RerunTestsOlderThanDaysVariableName) ?? "0",
                    out var rerunTestsOlderThanDays
                );

            rerunTestsOlderThanDays =
                Math
                    .Max(
                        0,
                        rerunTestsOlderThanDays
                    );

            DiagnosticResponse diagnosticResponse;

            var context =
                TestContext
                    .CurrentContext;

            var runCheckRequest =
                context
                    .ToRunCheckRequest(rerunTestsOlderThanDays);

            if (!string.IsNullOrEmpty(NotAgainUrl))
            {
                diagnosticResponse =
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

            if (!diagnosticResponse.IgnoreThisTest)
            {
                Stopwatch
                    .Start();
            }
            else
            {
                _submitResult = false;
                
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