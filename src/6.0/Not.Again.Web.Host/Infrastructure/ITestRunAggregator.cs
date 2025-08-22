using System.Threading;
using System.Threading.Tasks;

namespace Not.Again.Web.Host.Infrastructure
{
    public interface ITestRunAggregator
    {
        Task GetTestResultsAsync(CancellationToken cancellationToken);
    }
}