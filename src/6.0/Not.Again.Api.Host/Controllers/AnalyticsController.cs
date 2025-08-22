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

        public AnalyticsController(
            ILogger<AnalyticsController> logger,
            ITestAssemblyGetter testAssemblyGetter,
            ITestRecordGetter testRecordGetter)
        {
            _logger = logger;
            _testAssemblyGetter = testAssemblyGetter;
            _testRecordGetter = testRecordGetter;
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
                        .Select(o => new TestAssemblyRecord
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
                        .Select(o => new TestDetails
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
    }
}