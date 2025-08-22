using System.Threading;
using System.Threading.Tasks;

namespace Not.Again.Web.Host.Infrastructure
{
    public interface IApiAdapter
    {
        Task<TResponse> HttpPostAsync<TRequest, TResponse>(
            TRequest request = default,
            CancellationToken cancellationToken = default);
    }
}