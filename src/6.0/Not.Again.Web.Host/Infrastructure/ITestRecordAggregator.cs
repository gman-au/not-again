using System;
using System.Threading;
using System.Threading.Tasks;
using Not.Again.Contracts;

namespace Not.Again.Web.Host.Infrastructure
{
    public interface ITestRecordAggregator
    {
        Task<GetTestRecordsResponse> GetTestRecordsAsync(Guid? assemblyId, CancellationToken cancellationToken);
    }
}