using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Sample.IntegrationTests.Infrastructure;
using Xunit;

namespace Sample.IntegrationTests.Specs
{
    [Collection(Collections.Default)]
    public class HttpClientWithDefaultIdentity
    {
        private readonly HttpClient _userHttpCient;

        public HttpClientWithDefaultIdentity(DefaultFixture fixture)
        {
            // You can create an HttpClient instance with a default identity
            _userHttpCient = fixture.Server.CreateClient()
                .WithDefaultIdentity(Identities.User);
        }

        [Fact]
        public async Task Is_Authorized_For_Valid_User()
        {
            var response = await _userHttpCient.GetAsync("api/values");

            await response.IsSuccessStatusCodeOrThrow();
        }

        [Fact]
        public async Task Is_Unauthorized_For_Invalid_User()
        {
            // Schema does not match
            var response = await _userHttpCient.GetAsync("api/values/schema");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
