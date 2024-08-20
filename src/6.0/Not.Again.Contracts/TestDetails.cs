using System.Collections.Generic;

namespace Not.Again.Contracts
{
    public class TestDetails
    {
        public string Id { get; set; }

        public string AssemblyQualifiedName { get; set; }

        public string ClassName { get; set; }

        public string FullName { get; set; }

        public string MethodName { get; set; }

        public string TestName { get; set; }

        public IEnumerable<object> Arguments { get; set; }

        public long Hash { get; set; }
    }
}