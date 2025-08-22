using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Not.Again.Api.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly ILogger<AnalyticsController> _logger;

        public AnalyticsController(
            ILogger<AnalyticsController> logger
        )
        {
            _logger = logger;
        }

        [HttpPost("GetAssemblies")]
        public async Task<ActionResult> GetAssemblies()
        {
            _logger
                .LogInformation("GetAssemblies received");

            var result = "OK";

            return new OkObjectResult(result);
        }
    }
}