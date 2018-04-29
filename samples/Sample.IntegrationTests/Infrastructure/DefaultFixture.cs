using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Sample.IntegrationTests.Infrastructure
{
    public class DefaultFixture : IDisposable
    {
        public DefaultFixture()
        {
            // Build the test server
            var host = new WebHostBuilder()
                .UseStartup<TestStartup>();

            Server = new TestServer(host);
        }

        public TestServer Server { get; }

        public void Dispose()
        {
            Server.Dispose();
        }
    }
}
