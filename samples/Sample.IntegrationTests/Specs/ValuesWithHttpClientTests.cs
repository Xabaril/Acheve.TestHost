using System;
using System.Net.Http;
using System.Threading.Tasks;
using Sample.IntegrationTests.Infrastructure;
using Xunit;

namespace Sample.IntegrationTests.Specs
{
    [Collection(Collections.Api)]
    public class ValuesWithHttpClientTests : IDisposable
    {
        private readonly HttpClient _userHttpCient;

        public ValuesWithHttpClientTests(TestHostFixture fixture)
        {
            // You can create an HttpClient instance with a default identity
            _userHttpCient = fixture.Server.CreateClient()
                .WithDefaultIdentity(Identities.User);
        }

        [Fact]
        public async Task WithHttpClientWithDefaultIdentity()
        {
            var response = await _userHttpCient.GetAsync("api/values");

            await response.IsSuccessStatusCodeOrThrow();
        }

        public void Dispose()
        {
            _userHttpCient.Dispose();
        }
    }
}