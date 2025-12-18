using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace Sample.IntegrationTests.Infrastructure
{
    public class WebApplicationFactoryFixture : WebApplicationFactory<TestStartup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseStartup<TestStartup>()
                .UseSolutionRelativeContentRoot("samples")
                .UseTestServer();
        }
    }
}
