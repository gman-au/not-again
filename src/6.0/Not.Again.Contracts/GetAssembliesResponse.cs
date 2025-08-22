using System.Collections.Generic;

namespace Not.Again.Contracts
{
    public class GetAssembliesResponse
    {
        public IEnumerable<TestAssemblyRecord> Assemblies { get; set; }
    }
}