using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Not.Again.Contracts;
using Not.Again.Interfaces;

namespace Not.Again.Api.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiagnosticController : ControllerBase
    {
        private readonly ILogger<DiagnosticController> _logger;
        private readonly IResultSubmitter _resultSubmitter;
        private readonly IRunChecker _runChecker;

        public DiagnosticController(
            ILogger<DiagnosticController> logger,
            IResultSubmitter resultSubmitter,
            IRunChecker runChecker
        )
        {
            _logger = logger;
            _resultSubmitter = resultSubmitter;
            _runChecker = runChecker;
        }

        [HttpPost("RunCheck")]
        public async Task<ActionResult> RunCheckAsync([FromBody] RunCheckRequest value)
        {
            _logger
                .LogInformation("RunCheckRequest received");

            var result =
                await
                    _runChecker
                        .GetLastAsync(value);

            return new OkObjectResult(result);
        }

        [HttpPost("ReportResult")]
        public async Task<ActionResult> ReportResultAsync([FromBody] SubmitResultRequest value)
        {
            _logger
                .LogInformation("SubmitResultRequest received");

            await
                _resultSubmitter
                    .SubmitResultAsync(value);

            return new OkResult();
        }
    }
}