using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.Logging;
using Moq;
using Not.Again.Contracts;
using Not.Again.Database;
using Not.Again.Domain;
using Not.Again.Enum;
using Not.Again.Interfaces;

namespace Not.Again.Tests.Unit
{
    public class RunCheckerTests
    {
        private readonly TestContext _context = new();

        [Fact]
        public async Task Request_No_Assembly_Check()
        {
            _context.ArrangeRequest();
            _context.ArrangeAssemblyNotFound();
            await _context.ActRunCheckAsync();
            _context.AssertAssemblyWasNotFound();
            _context.AssertTestNotIgnored();
        }

        [Fact]
        public async Task Request_Is_Assembly_Check()
        {
            _context.ArrangeRequest();
            _context.ArrangeAssemblyFound();
            await _context.ActRunCheckAsync();
            _context.AssertAssemblyWasFound();
        }

        [Fact]
        public async Task Request_Is_Assembly_No_Record_Check()
        {
            _context.ArrangeRequest();
            _context.ArrangeAssemblyFound();
            _context.ArrangeTestRecordNotFound();
            await _context.ActRunCheckAsync();
            _context.AssertAssemblyWasFound();
            _context.AssertRecordWasNotFound();
            _context.AssertTestNotIgnored();
        }

        [Fact]
        public async Task Request_Is_Assembly_Record_Check()
        {
            _context.ArrangeRequest();
            _context.ArrangeAssemblyFound();
            _context.ArrangeTestRecordFound();
            await _context.ActRunCheckAsync();
            _context.AssertAssemblyWasFound();
            _context.AssertRecordWasFound();
        }

        [Fact]
        public async Task Request_Is_Assembly_Is_Record_No_Run_Check()
        {
            _context.ArrangeRequest();
            _context.ArrangeAssemblyFound();
            _context.ArrangeTestRecordFound();
            _context.ArrangeTestRunNotFound();
            await _context.ActRunCheckAsync();
            _context.AssertAssemblyWasFound();
            _context.AssertRecordWasFound();
            _context.AssertRunWasNotFound();
            _context.AssertTestNotIgnored();
        }

        [Fact]
        public async Task Request_Is_Assembly_Is_Record_Is_Run_Is_Passed_Older_Check()
        {
            _context.ArrangeRequest();
            _context.ArrangeAssemblyFound();
            _context.ArrangeTestRecordFound();
            _context.ArrangeTestRunOlderPassedFound();
            await _context.ActRunCheckAsync();
            _context.AssertAssemblyWasFound();
            _context.AssertRecordWasFound();
            _context.AssertRunWasFound();
            _context.AssertOnlyOlderRunFound();
            _context.AssertTestNotIgnored();
        }

        [Fact]
        public async Task Request_Is_Assembly_Is_Record_Is_Run_Is_Passed_New_Check()
        {
            _context.ArrangeRequest();
            _context.ArrangeAssemblyFound();
            _context.ArrangeTestRecordFound();
            _context.ArrangeTestRunNewerPassedFound();
            await _context.ActRunCheckAsync();
            _context.AssertAssemblyWasFound();
            _context.AssertRecordWasFound();
            _context.AssertRunWasFound();
            _context.AssertNewerRunFound();
            _context.AssertTestIgnored();
        }

        [Fact]
        public async Task Request_Is_Assembly_Is_Record_Is_Run_Is_Passed_Older_Check_Null_Interval()
        {
            _context.ArrangeRequest();
            _context.ArrangeNullDayInterval();
            _context.ArrangeAssemblyFound();
            _context.ArrangeTestRecordFound();
            _context.ArrangeTestRunNewerPassedFound();
            await _context.ActRunCheckAsync();
            _context.AssertAssemblyWasFound();
            _context.AssertRecordWasFound();
            _context.AssertRunWasFound();
            _context.AssertIntervalNull();
            _context.AssertTestNotIgnored();
        }

        private class TestContext
        {
            private const string MyTestAssemblyQualifiedName = "My.Test.Assembly";
            private const string MyTestAssemblyClassName = "My.Test.Assembly.MyClass";
            private const string MyTestName = "MyTestName";
            private const int DayIntervalRequest = 20;
            private readonly Mock<IArgumentDelimiter> _argumentDelimiter;

            private readonly IFixture _fixture;
            private readonly Mock<IMessageFormatter> _messageFormatter;
            private readonly RunChecker _sut;
            private readonly Mock<ITestAssemblyGetter> _testAssemblyGetter;
            private readonly Mock<ITestRecordGetter> _testRecordGetter;
            private readonly Mock<ITestRunGetter> _testRunGetter;
            private RunCheckRequest _request;
            private DiagnosticResponse _result;

            public TestContext()
            {
                _fixture =
                    new Fixture()
                        .Customize(new AutoMoqCustomization());

                _fixture
                    .Behaviors
                    .OfType<ThrowingRecursionBehavior>()
                    .ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));

                _fixture
                    .Behaviors
                    .Add(new OmitOnRecursionBehavior());

                _argumentDelimiter = new Mock<IArgumentDelimiter>();
                _testRecordGetter = new Mock<ITestRecordGetter>();
                _testAssemblyGetter = new Mock<ITestAssemblyGetter>();
                _testRunGetter = new Mock<ITestRunGetter>();
                _messageFormatter = new Mock<IMessageFormatter>();
                var logger = new Mock<ILogger<RunChecker>>();

                _sut =
                    new RunChecker(
                        _testAssemblyGetter.Object,
                        _testRecordGetter.Object,
                        _testRunGetter.Object,
                        _argumentDelimiter.Object,
                        _messageFormatter.Object,
                        logger.Object
                    );
            }

            public void ArrangeRequest()
            {
                var testDetails =
                    _fixture
                        .Build<TestDetails>()
                        .With(
                            o => o.AssemblyQualifiedName,
                            MyTestAssemblyQualifiedName
                        )
                        .With(
                            o => o.FullName,
                            MyTestName
                        )
                        .Create();

                _request =
                    _fixture
                        .Build<RunCheckRequest>()
                        .With(
                            o => o.TestDetails,
                            testDetails
                        )
                        .With(
                            o => o.RerunTestsOlderThanDays,
                            DayIntervalRequest
                        )
                        .Create();
            }

            public void ArrangeNullDayInterval() => _request.RerunTestsOlderThanDays = null;

            public void ArrangeAssemblyNotFound()
            {
                _testAssemblyGetter
                    .Setup(o => o.GetAsync(It.IsAny<TestAssembly>()))
                    .ReturnsAsync((TestAssembly)null);
            }

            public void ArrangeTestRecordNotFound()
            {
                _testRecordGetter
                    .Setup(
                        o => o.GetAsync(
                            It.IsAny<Guid>(),
                            It.IsAny<TestRecord>(),
                            It.IsAny<bool>()
                        )
                    )
                    .ReturnsAsync((TestRecord)null);
            }

            public void ArrangeTestRunNotFound()
            {
                _testRunGetter
                    .Setup(o => o.GetLastRunAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((TestRun)null);
            }

            public void ArrangeAssemblyFound()
            {
                var assembly =
                    _fixture
                        .Build<TestAssembly>()
                        .With(
                            o => o.TestAssemblyName,
                            MyTestAssemblyQualifiedName
                        )
                        .Create();

                _testAssemblyGetter
                    .Setup(o => o.GetAsync(It.IsAny<TestAssembly>()))
                    .ReturnsAsync(assembly);
            }

            public void ArrangeTestRecordFound()
            {
                var testRecord =
                    _fixture
                        .Build<TestRecord>()
                        .With(
                            o => o.FullName,
                            MyTestName
                        )
                        .With(
                            o => o.ClassName,
                            MyTestAssemblyClassName
                        )
                        .Create();

                _testRecordGetter
                    .Setup(
                        o => o.GetAsync(
                            It.IsAny<Guid>(),
                            It.IsAny<TestRecord>(),
                            It.IsAny<bool>()
                        )
                    )
                    .ReturnsAsync(testRecord);
            }

            public void ArrangeTestRunNewerPassedFound()
            {
                var testRun =
                    _fixture
                        .Build<TestRun>()
                        .With(
                            o => o.RunDate,
                            DateTime.UtcNow.AddDays(-10)
                        )
                        .With(
                            o => o.Result,
                            (int)TestResultEnum.Passed
                        )
                        .Create();

                _testRunGetter
                    .Setup(o => o.GetLastRunAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(testRun);
            }

            public void ArrangeTestRunOlderPassedFound()
            {
                var testRun =
                    _fixture
                        .Build<TestRun>()
                        .With(
                            o => o.RunDate,
                            DateTime.UtcNow.AddDays(-30)
                        )
                        .With(
                            o => o.Result,
                            (int)TestResultEnum.Passed
                        )
                        .Create();

                _testRunGetter
                    .Setup(o => o.GetLastRunAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(testRun);
            }

            public async Task ActRunCheckAsync()
            {
                _result =
                    await
                        _sut
                            .GetLastAsync(_request);
            }

            public void AssertAssemblyWasFound()
            {
                _messageFormatter
                    .Verify(
                        o => o.EncapsulateNotAgainMessage(
                            "No prior record of this test assembly [My.Test.Assembly], the test [MyTestName] should NOT be ignored"
                        ), Times.Never
                    );

                _argumentDelimiter
                    .Verify(
                        o => o.Perform(It.IsAny<IEnumerable<object>>()),
                        Times.Once
                    );
            }

            public void AssertAssemblyWasNotFound()
            {
                _messageFormatter
                    .Verify(
                        o => o.EncapsulateNotAgainMessage(
                            "No prior record of this test assembly [My.Test.Assembly], the test [MyTestName] should NOT be ignored"
                        ), Times.Once
                    );

                _argumentDelimiter
                    .Verify(
                        o => o.Perform(It.IsAny<IEnumerable<object>>()),
                        Times.Never
                    );

                _testRecordGetter
                    .Verify(
                        o => o.GetAsync(
                            It.IsAny<Guid>(),
                            It.IsAny<TestRecord>(),
                            It.IsAny<bool>()
                        ), Times.Never
                    );
            }

            public void AssertRecordWasNotFound()
            {
                _messageFormatter
                    .Verify(
                        o => o.EncapsulateNotAgainMessage(
                            "The test [MyTestName] is either new or has been modified since last run - it should NOT be ignored"
                        ), Times.Once
                    );
            }

            public void AssertRecordWasFound()
            {
                _messageFormatter
                    .Verify(
                        o => o.EncapsulateNotAgainMessage(
                            "The test [MyTestName] is either new or has been modified since last run - it should NOT be ignored"
                        ), Times.Never
                    );
            }

            public void AssertRunWasNotFound()
            {
                _messageFormatter
                    .Verify(
                        o => o.EncapsulateNotAgainMessage(
                            "No prior test run found for this test record [MyTestName] - it should NOT be ignored"
                        ), Times.Once
                    );
            }

            public void AssertRunWasFound()
            {
                _messageFormatter
                    .Verify(
                        o => o.EncapsulateNotAgainMessage(
                            "No prior test run found for this test record [MyTestName] - it should NOT be ignored"
                        ), Times.Never
                    );
            }

            public void AssertIntervalNull()
            {
                _messageFormatter
                    .Verify(
                        o => o.EncapsulateNotAgainMessage(
                            "No re-run interval was specified in the request for test [MyTestName] - it should NOT be ignored"
                        ), Times.Once
                    );
            }

            public void AssertNewerRunFound()
            {
                _messageFormatter
                    .Verify(
                        o => o.EncapsulateNotAgainMessage(
                            "Last run for test [MyTestName] did not exceed the specified interval of 20 days - it should be ignored"
                        ), Times.Once
                    );
            }

            public void AssertOnlyOlderRunFound()
            {
                _messageFormatter
                    .Verify(
                        o => o.EncapsulateNotAgainMessage(
                            "Last run for test [MyTestName] exceeded the specified interval of 20 days - it should NOT be ignored"
                        ), Times.Once
                    );
            }

            public void AssertTestNotIgnored() => Assert.False(_result.IgnoreThisTest);

            public void AssertTestIgnored() => Assert.True(_result.IgnoreThisTest);
        }
    }
}