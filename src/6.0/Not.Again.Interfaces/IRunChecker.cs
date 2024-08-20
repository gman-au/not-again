using System.Threading.Tasks;
using Not.Again.Contracts;

namespace Not.Again.Interfaces
{
    public interface IRunChecker
    {
        Task<bool> GetLastAsync(RunCheckRequest request);
    }
}