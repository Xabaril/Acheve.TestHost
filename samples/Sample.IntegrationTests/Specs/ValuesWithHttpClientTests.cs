using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Sample.IntegrationTests.Infrastructure;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sample.IntegrationTests.Specs
{
    public abstract class ValuesWithHttpClientTests : IDisposable
    {
        private readonly HttpClient _userHttpCient;

        protected ValuesWithHttpClientTests(TestServer server)
        {
            // You can create an HttpClient instance with a default identity
            _userHttpCient = server.CreateClient()
                .WithDefaultIdentity(Identities.User);
        }

        [Fact]
        public async Task WithHttpClientWithDefaultIdentity()
        {
            var response = await _userHttpCient.GetAsync("api/values");

            await response.IsSuccessStatusCodeOrThrow();
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        public void Dispose()
        {
            _userHttpCient.Dispose();
        }
    }

    [Collection(nameof(ApiCollection))]
    public class ValuesWithHttpClientTestHostTests : ValuesWithHttpClientTests
    {
        public ValuesWithHttpClientTestHostTests(TestHostFixture fixture) : base(fixture.Server) { }
    }

    [Collection(nameof(WebApplicationFactoryApiCollection))]
    public class ValuesWithHttpClientWebApplicationFactoryTests : ValuesWithHttpClientTests
    {
        public ValuesWithHttpClientWebApplicationFactoryTests(WebApplicationFactoryFixture fixture) : base(fixture.Server) { }
    }
}