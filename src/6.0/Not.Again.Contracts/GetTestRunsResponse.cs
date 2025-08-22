using System.Collections.Generic;

namespace Not.Again.Contracts
{
    public class GetTestRunsResponse
    {
        public IEnumerable<ReportableTestRun> TestRuns { get; set; }
    }
}