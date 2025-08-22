using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Not.Again.Database;
using Not.Again.Infrastructure;
using Not.Again.Interfaces;

namespace Not.Again.Api.Host.Injection
{
    public static class IoCNotAgain
    {
        public static IServiceCollection AddNotAgainServices(
            this IServiceCollection services,
            IConfigurationRoot configuration
        )
        {
            services
                .AddTransient<IResultSubmitter, ResultSubmitter>()
                .AddTransient<ITestAssemblyGetter, TestAssemblyGetter>()
                .AddTransient<ITestRecordGetter, TestRecordGetter>()
                .AddTransient<ITestRunGetter, TestRunGetter>()
                .AddTransient<ITestAssemblyPutter, TestAssemblyPutter>()
                .AddTransient<ITestRecordPutter, TestRecordPutter>()
                .AddTransient<ITestRunPutter, TestRunPutter>()
                .AddTransient<IRunChecker, RunChecker>()
                .AddTransient<IArgumentDelimiter, ArgumentDelimiter>()
                .AddTransient<IMessageFormatter, MessageFormatter>();

            var connectionStringSqlServer =
                configuration
                    .GetConnectionString("NOT-AGAIN-SQL-SERVER");

            var connectionStringPostgreSql =
                configuration
                    .GetConnectionString("NOT-AGAIN-SQL-POSTGRESQL");

            if (!string.IsNullOrEmpty(connectionStringSqlServer))
            {
                services
                    .AddDbContext<NotAgainDbContext>(o =>
                        o.UseSqlServer(connectionStringSqlServer)
                    );

                return services;
            }

            if (!string.IsNullOrEmpty(connectionStringPostgreSql))
            {
                services
                    .AddDbContext<NotAgainDbContext>(o =>
                        o.UseNpgsql(connectionStringPostgreSql)
                    );

                return services;
            }

            throw new Exception(StandardMessages.NoConnectionStringMessage);
        }
    }
}