using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Not.Again.Database;
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
                .AddTransient<ITestRecordGetter, TestRecordGetter>()
                .AddTransient<ITestAssemblyGetter, TestAssemblyGetter>()
                .AddTransient<IRunChecker, RunChecker>()
                .AddTransient<IArgumentDelimiter, ArgumentDelimiter>()
                .AddTransient<IMessageFormatter, MessageFormatter>();

            services
                .AddDbContext<NotAgainDbContext>(
                    o =>
                        o.UseSqlServer(configuration.GetConnectionString("NOT-AGAIN"))
                );

            return services;
        }
    }
}