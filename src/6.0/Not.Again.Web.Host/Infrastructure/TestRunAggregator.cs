using System;
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

        public async Task<GetTestRunsResponse> GetTestRunsAsync(
            Guid? assemblyId,
            Guid? testRecordId,
            CancellationToken cancellationToken
        )
        {
            var path = "Analytics/GetTestRuns";

            var request = new GetTestRunsRequest
            {
                AssemblyId = assemblyId,
                TestRecordId = testRecordId
            };

            var response =
                await
                    _apiAdapter
                        .HttpPostAsync<GetTestRunsRequest, GetTestRunsResponse>(
                            path,
                            request,
                            cancellationToken
                        );

            return response;
        }
    }
}