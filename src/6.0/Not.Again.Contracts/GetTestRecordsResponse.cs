using System.Collections.Generic;

namespace Not.Again.Contracts
{
    public class GetTestRecordsResponse
    {
        public IEnumerable<TestDetails> TestRecords { get; set; }
    }
}