using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Not.Again.Infrastructure;
using Not.Again.Interfaces;

namespace Not.Again.Tests.Unit
{
    internal class BaseResolverTestContext
    {
        protected readonly ServiceCollection Services;
        protected IConfigurationRoot Configuration;
        protected IServiceProvider ServiceProvider;

        protected BaseResolverTestContext()
        {
            Services = new ServiceCollection();
        }

        public void ArrangeConnectionString()
        {
            Environment.SetEnvironmentVariable(
                StandardConstants.ConnectionStringVariableName,
                "server=127.0.0.1;"
            );

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .Build();
        }

        public void AssertResolvedServices()
        {
            Assert
                .NotNull(ServiceProvider
                    .GetService<IResultSubmitter>());
            
            Assert
                .NotNull(ServiceProvider
                    .GetService<IRunChecker>());
        }

        public void AssertConnectionStringException(Exception exception)
        {
            Assert.Equal(
                StandardMessages.NoConnectionStringMessage,
                exception.Message
            );
        }
    }
}