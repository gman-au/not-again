using System;

namespace Not.Again.Contracts
{
    public class ReportableTestRun
    {
        public Guid TestAssemblyId { get; set; }

        public string TestAssemblyShortName { get; set; }

        public string TestId { get; set; }

        public string TestName { get; set; }

        public int Status { get; set; }

        public long Duration { get; set; }

        public DateTime RunDate { get; set; }

        public string TestRunner { get; set; }
    }
}