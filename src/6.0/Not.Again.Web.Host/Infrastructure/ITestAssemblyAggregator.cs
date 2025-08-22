using System.Threading;
using System.Threading.Tasks;
using Not.Again.Contracts;

namespace Not.Again.Web.Host.Infrastructure
{
    public interface ITestAssemblyAggregator
    {
        Task<GetAssembliesResponse> GetAllTestAssembliesAsync(CancellationToken cancellationToken);
    }
}