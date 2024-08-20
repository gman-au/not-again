namespace Not.Again.Contracts
{
    public class RunCheckRequest
    {
        public TestDetails TestDetails { get; set; }

        public int? RerunTestsOlderThanDays { get; set; }
    }
}