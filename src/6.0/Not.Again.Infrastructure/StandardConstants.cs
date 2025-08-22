namespace Not.Again.Infrastructure
{
    public static class StandardConstants
    {
        public const string ConnectionStringVariableNameSqlServer = "ConnectionStrings__NOT-AGAIN-SQL-SERVER";
        public const string ConnectionStringVariableNamePostgreSql = "ConnectionStrings__NOT-AGAIN-SQL-POSTGRESQL";

        public const string BaseUrlVariableName = "NOT_AGAIN_URL";
        public const string RerunTestsOlderThanDaysVariableName = "RERUN_TESTS_OLDER_THAN_DAYS";
        public const int DefaultRerunTestsOlderThanDays = 0;

        public const string NUnitRunnerType = "NUnit";
    }
}