using System;
using System.Threading;
using System.Threading.Tasks;
using Not.Again.Contracts;

namespace Not.Again.Web.Host.Infrastructure
{
    public class TestRecordAggregator : ITestRecordAggregator
    {
        private readonly IApiAdapter _apiAdapter;

        public TestRecordAggregator(IApiAdapter apiAdapter)
        {
            _apiAdapter = apiAdapter;
        }

        public async Task<GetTestRecordsResponse> GetTestRecordsAsync(Guid? assemblyId, CancellationToken cancellationToken)
        {
            var path = "Analytics/GetTestRecords";

            if (assemblyId.HasValue)
                path += $"/{assemblyId.Value}";

            var response =
                await
                    _apiAdapter
                        .HttpGetAsync<GetTestRecordsResponse>(path, cancellationToken);

            return response;
        }
    }
}