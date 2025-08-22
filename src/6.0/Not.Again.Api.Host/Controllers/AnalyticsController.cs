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

        public AnalyticsController(
            ILogger<AnalyticsController> logger,
            ITestAssemblyGetter testAssemblyGetter)
        {
            _logger = logger;
            _testAssemblyGetter = testAssemblyGetter;
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
    }
}