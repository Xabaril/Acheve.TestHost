using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sample.IntegrationTests.Infrastructure
{
    public class TestHostFixture : IDisposable, IAsyncLifetime
    {
        private IWebHost _host;

        public TestServer Server => _host.GetTestServer();

        public void Dispose()
        {
            Server.Dispose();
            _host.Dispose();
        }

        /// <inheritdoc />
        public async Task InitializeAsync()
        {
            _host = new WebHostBuilder()
                .UseTestServer()
                .UseStartup<TestStartup>()
                .Build();

            await _host.StartAsync();
        }

        /// <inheritdoc />
        public Task DisposeAsync()
        {
            // Nothing here
            return Task.CompletedTask;
        }
    }
}
