using System.Threading;
using System.Threading.Tasks;

namespace Not.Again.Web.Host.Infrastructure
{
    public interface IApiAdapter
    {
        Task<TResponse> HttpPostAsync<TRequest, TResponse>(
            string path,
            TRequest request = default,
            CancellationToken cancellationToken = default);

        Task<TResponse> HttpGetAsync<TResponse>(
            string path,
            CancellationToken cancellationToken = default);
    }
}