using System.Threading.Tasks;
using Not.Again.Contracts;

namespace Not.Again.Interfaces
{
    public interface IResultSubmitter
    {
        Task SubmitResultAsync(SubmitResultRequest request);
    }
}