using System;
using Microsoft.Extensions.DependencyInjection;
using Not.Again.Api.Host.Injection;

namespace Not.Again.Tests.Unit
{
	public class DependencyInjectionTests
	{
		private readonly TestContext _context = new();

        [Fact]
		public void Can_resolve_api_services_valid_connection_string()
		{
			_context.ArrangeConnectionString();
			_context.ActInjectHostServices();
			_context.AssertResolvedServices();
		}

        [Fact]
		public void Cannot_resolve_api_services_without_connection_string()
		{
            var exception = Assert.Throws<Exception>(() => _context.ActInjectHostServices());
            _context.AssertConnectionStringException(exception);
        }

		private class TestContext : BaseResolverTestContext
		{
			public void ActInjectHostServices()
			{
				Services.AddNotAgainServices(Configuration);

				Services.AddLogging();

				ServiceProvider = Services.BuildServiceProvider();
			}
        }
	}
}