using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Not.Again.Api.Host.Extensions;
using Not.Again.Contracts;
using Not.Again.Interfaces;

namespace Not.Again.Api.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly ILogger<AnalyticsController> _logger;
        private readonly ITestAssemblyGetter _testAssemblyGetter;
        private readonly ITestRecordGetter _testRecordGetter;
        private readonly ITestRunGetter _testRunGetter;

        public AnalyticsController(
            ILogger<AnalyticsController> logger,
            ITestAssemblyGetter testAssemblyGetter,
            ITestRecordGetter testRecordGetter,
            ITestRunGetter testRunGetter)
        {
            _logger = logger;
            _testAssemblyGetter = testAssemblyGetter;
            _testRecordGetter = testRecordGetter;
            _testRunGetter = testRunGetter;
        }

        [HttpGet("GetAssemblies")]
        public async Task<GetAssembliesResponse> GetAssemblies()
        {
            _logger
                .LogInformation("GetAssemblies received");

            var assemblies =
                await
                    _testAssemblyGetter
                        .GetAsync();

            var result = new GetAssembliesResponse
            {
                Assemblies =
                    assemblies
                        .Select(o =>
                            new TestAssemblyRecord
                            {
                                TestAssemblyId = o.TestAssemblyId,
                                TestAssemblyShortName = o.TestAssemblyName.ToShortName()
                            })
                        .ToList()
            };

            return
                result;
        }

        [HttpGet("GetTestRecords/{assemblyId:guid?}")]
        public async Task<GetTestRecordsResponse> GetTestRecords(Guid? assemblyId)
        {
            _logger
                .LogInformation("GetTestRecords received");

            var testRecords =
                await
                    _testRecordGetter
                        .GetAsync(assemblyId);

            var result = new GetTestRecordsResponse
            {
                TestRecords =
                    testRecords
                        .Select(o =>
                            new TestDetails
                            {
                                Id = o.TestRecordId.ToString(),
                                ClassName = o.ClassName,
                                FullName = o.FullName,
                                MethodName = o.MethodName,
                                TestName = o.TestName,
                                Arguments = o.DelimitedTestArguments.Split(','),
                                Hash = o.LastHash
                            })
                        .ToList()
            };

            return
                result;
        }

        [HttpPost("GetTestRuns")]
        public async Task<GetTestRunsResponse> GetTestRecords([FromBody] GetTestRunsRequest request)
        {
            _logger
                .LogInformation("GetTestRuns received");

            var testRecords =
                await
                    _testRunGetter
                        .GetTestRunsAsync(
                            request.AssemblyId,
                            request.TestRecordId
                        );

            var result = new GetTestRunsResponse
            {
                TestRuns =
                    testRecords
                        .Select(o =>
                            new ReportableTestRun
                            {
                                TestAssemblyId = o.TestRecord.TestAssembly.TestAssemblyId,
                                TestAssemblyShortName = o.TestRecord.TestAssembly.TestAssemblyName.ToShortName(),
                                TestRunner = o.TestRecord.TestAssembly.TestRunner,
                                TestId = o.TestRecord.TestRecordId.ToString(),
                                TestName = o.TestRecord.TestName,
                                Status = o.Result,
                                Duration = o.TotalDuration,
                                RunDate = o.RunDate
                            })
                        .ToList()
            };

            return
                result;
        }
    }
}