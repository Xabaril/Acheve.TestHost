using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Sample.IntegrationTests.Infrastructure
{
    public class TestHostFixture : IDisposable
    {
        public TestHostFixture()
        {
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
