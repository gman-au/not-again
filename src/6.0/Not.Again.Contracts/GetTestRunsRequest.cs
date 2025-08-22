using System;

namespace Not.Again.Contracts
{
    public class GetTestRunsRequest
    {
        public Guid? AssemblyId { get; set; }

        public Guid? TestRecordId { get; set; }
    }
}