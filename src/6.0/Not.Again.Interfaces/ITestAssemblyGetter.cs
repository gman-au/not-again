using System.Collections.Generic;
using System.Threading.Tasks;
using Not.Again.Domain;

namespace Not.Again.Interfaces
{
    public interface ITestAssemblyGetter
    {
        Task<IEnumerable<TestAssembly>> GetAsync();

        Task<TestAssembly> GetAsync(TestAssembly testAssembly);
    }
}