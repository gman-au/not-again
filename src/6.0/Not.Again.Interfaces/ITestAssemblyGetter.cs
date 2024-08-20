using System.Threading.Tasks;
using Not.Again.Domain;

namespace Not.Again.Interfaces
{
    public interface ITestAssemblyGetter
    {
        Task<TestAssembly> GetAsync(TestAssembly testAssembly);
    }
}