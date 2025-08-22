using System;
using System.Threading;
using System.Threading.Tasks;

namespace Not.Again.Web.Host.Infrastructure
{
    public class TestRunAggregator : ITestRunAggregator
    {
        private readonly IApiAdapter _apiAdapter;

        public TestRunAggregator(IApiAdapter apiAdapter)
        {
            _apiAdapter = apiAdapter;
        }

        public Task GetTestResultsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}