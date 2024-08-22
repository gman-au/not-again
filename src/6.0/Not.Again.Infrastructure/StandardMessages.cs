namespace Not.Again.Infrastructure
{
    public static class StandardMessages
    {
        public const string GenericApiClientErrorMessage = "Warning - there was an error when attempting to connect to the API; please check your configuration.";
        public const string IgnoringThisTestMessage = "This test has been run previously - ignoring";
        public const string NoUrlEnvVariableSuppliedMessage = "No NOT_AGAIN_URL endpoint was supplied (via environment variable) for interaction with NotAgain service";
        public const string NoConnectionStringMessage = $"A connection string has not been defined. Please ensure an environment variable \"{StandardConstants.ConnectionStringVariableName}\" has been supplied.";

        public const string AssemblyNotFound = "No prior record of this test assembly [{0}], the test [{1}] should NOT be ignored";
        public const string RecordNotFound = "The test [{0}] is either new or has been modified since last run - it should NOT be ignored";
        public const string RunNotFound = "No prior test run found for this test record [{0}] - it should NOT be ignored";
        public const string NewerRunFound = "Last run for test [{0}] did not exceed the specified interval of {1} days - it should be ignored";
        public const string OnlyOlderRunFound = "Last run for test [{0}] exceeded the specified interval of {1} days - it should NOT be ignored";
        public const string LastRunFailed = "Last run for test [{0}] failed - it should NOT be ignored";
        public const string NoIntervalSpecifiedForFoundRun = "No re-run interval was specified in the request for test [{0}] - it should NOT be ignored";
    }
}