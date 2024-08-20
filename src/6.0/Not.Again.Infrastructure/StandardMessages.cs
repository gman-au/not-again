namespace Not.Again.Infrastructure
{
    public static class StandardMessages
    {
        public const string IgnoringThisTestMessage = "This test has been run previously - ignoring";
        public const string RunningThisTestMessage = "No previous applicable test run reported, running this test...";
        public const string NoUrlEnvVariableSuppliedMessage = "No NOT_AGAIN_URL endpoint was supplied (via environment variable) for interaction with NotAgain service";
        public const string NoConnectionStringMessage = $"A connection string has not been defined. Please ensure an environment variable \"{StandardConstants.ConnectionStringVariableName}\" has been supplied.";
    }
}