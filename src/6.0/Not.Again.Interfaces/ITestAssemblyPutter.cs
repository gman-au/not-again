using System.Threading.Tasks;
using Not.Again.Domain;

namespace Not.Again.Interfaces
{
    public interface ITestAssemblyPutter
    {
        Task<TestAssembly> AddOrUpdateTestAssemblyAsync(
            string assemblyName,
            string testRunner
        );
    }
}