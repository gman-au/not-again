using System.Threading;
using System.Threading.Tasks;
using Not.Again.Contracts;

namespace Not.Again.Web.Host.Infrastructure
{
    public class TestRunAggregator : ITestRunAggregator
    {
        private readonly IApiAdapter _apiAdapter;

        public TestRunAggregator(IApiAdapter apiAdapter)
        {
            _apiAdapter = apiAdapter;
        }

        public async Task<GetAssembliesResponse> GetAllTestAssembliesAsync(CancellationToken cancellationToken)
        {
            var response =
                await
                    _apiAdapter
                        .HttpGetAsync<GetAssembliesResponse>("Analytics/GetAssemblies", cancellationToken);

            return response;
        }
    }
}