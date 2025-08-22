using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Not.Again.Web.Host.Infrastructure;
using Not.Again.Web.Host.Options;

namespace Not.Again.Web.Host.Injection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddNotAgainApi(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddTransient<IApiAdapter, ApiAdapter>()
                .AddTransient<ITestAssemblyAggregator, TestAssemblyAggregator>()
                .AddTransient<ITestRecordAggregator, TestRecordAggregator>();

            services
                .Configure<WebHostConfigurationOptions>(
                    configuration
                        .GetSection(nameof(WebHostConfigurationOptions))
                );

            return services;
        }
    }
}