using Acheve.AspNetCore.TestHost.Security;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Sample.Api.Controllers;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sample.IntegrationTests
{
    public class VauesWithDefaultUserTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _userHttpCient;

        public VauesWithDefaultUserTests()
        {
            // Build the test server
            var host = new WebHostBuilder()
                .UseStartup<TestStartup>();

            _server = new TestServer(host);

            // You can create an HttpClient instance with a default identity
            _userHttpCient = _server.CreateClient()
                .WithDefaultIdentity(Identities.User);
        }

        [Fact]
        public async Task WithHttpClientWithDefautIdentity()
        {
            var response = await _userHttpCient.GetAsync("api/values");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task WithRequestBuilder()
        {
            // Or you can create a request and assign the identity to the RequestBuilder
            var response = await _server.CreateHttpApiRequest<ValuesController>(controller=>controller.Values())
                .WithIdentity(Identities.User)
                .GetAsync();

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task WithEmptyRequestBuilder()
        {
            // Or you can create a request and assign the identity to the RequestBuilder
            var response = await _server.CreateHttpApiRequest<ValuesController>(controller => controller.Values())
                .WithIdentity(Identities.Empty)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Anonymous()
        {
            var response = await _server.CreateHttpApiRequest<ValuesController>(controller => controller.PublicValues())
                .GetAsync();

            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _server.Dispose();
            _userHttpCient.Dispose();
        }
    }
}
