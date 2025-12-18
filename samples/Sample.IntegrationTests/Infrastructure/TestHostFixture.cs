using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sample.IntegrationTests.Infrastructure
{
    public sealed class TestHostFixture : IDisposable, IAsyncLifetime
    {
        private IHost _host;

        public TestServer Server => _host.GetTestServer();

        public void Dispose()
        {
            Server.Dispose();
            _host.Dispose();
        }

        /// <inheritdoc />
        public async Task InitializeAsync()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureWebHost(webHostBuilder =>
                {
                    webHostBuilder
                        .UseStartup<TestStartup>()
                        .UseTestServer();
                }).Build();

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
