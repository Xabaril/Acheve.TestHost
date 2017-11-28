using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.Acheve.TestHost.TestBuilders
{
    public class TestServerBuilder
    {
        private readonly IWebHostBuilder _webHost = new WebHostBuilder();

        public TestServer Build()
        {
            return new TestServer(_webHost);
        }

        public TestServerBuilder UseDefaultStartup()
        {
            _webHost.UseStartup<Startup>();

            return this;
        }

        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
            }
            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            {

            }
        }
    }
}
