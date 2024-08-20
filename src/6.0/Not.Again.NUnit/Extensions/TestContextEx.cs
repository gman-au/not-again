using System;
using Not.Again.Contracts;
using Not.Again.Infrastructure;
using NUnit.Framework;

namespace Not.Again.NUnit.Extensions
{
    public static class TestContextEx
    {
        internal static RunCheckRequest ToRunCheckRequest(this TestContext context)
        {
            return new RunCheckRequest
            {
                TestDetails = GetTestDetails(context),
                RerunTestsOlderThanDays = 10
            };
        }

        internal static SubmitResultRequest ToSubmitResultRequest(this TestContext context, long totalMilliseconds)
        {
            return new SubmitResultRequest
            {
                TestDetails = GetTestDetails(context),
                TestResultDetails = GetTestResultDetails(
                    context,
                    totalMilliseconds
                )
            };
        }

        private static TestDetails GetTestDetails(this TestContext context)
        {
            var test = context.Test;

            var className =
                context
                    .Test
                    .ClassName;

            var methodName =
                context
                    .Test
                    .ClassName;

            var assemblyQualifiedName =
                AssemblyLoader
                    .GetTypeFromAssembly(className)?
                    .AssemblyQualifiedName;

            var hash =
                AssemblyMethodHasher
                    .CalculateTestMethodHash(
                        className,
                        methodName
                    );

            return new TestDetails
            {
                Id = test.ID,
                ClassName = className,
                FullName = test.FullName,
                MethodName = methodName,
                TestName = test.Name,
                Arguments = test.Arguments,
                Hash = hash.GetValueOrDefault(0),
                AssemblyQualifiedName = assemblyQualifiedName,
            };
        }

        private static TestResultDetails GetTestResultDetails(this TestContext context, long totalMilliseconds)
        {
            var result = context.Result;

            return new TestResultDetails
            {
                Status = (int)result.Outcome.Status,
                Duration = totalMilliseconds,
                RunDate = DateTime.UtcNow
            };
        }
    }
}