using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Sample.Api.Controllers;
using Sample.IntegrationTests.Infrastructure;
using Xunit;

namespace Sample.IntegrationTests.Specs
{
    [Collection(Collections.Default)]
    public class ServerCreateHttpApiRequest
    {
        private readonly DefaultFixture _fixture;
        
        public ServerCreateHttpApiRequest(DefaultFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task WithRequestBuilder()
        {
            // Or you can create a request and assign the identity to the RequestBuilder
            var response = await _fixture.Server
                .CreateHttpApiRequest<ValuesController>(controller => controller.Values())
                .WithIdentity(Identities.User)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }

        [Fact]
        public async Task WithEmptyRequestBuilder_Unauthorized()
        {
            var response = await _fixture.Server
                .CreateHttpApiRequest<ValuesController>(controller => controller.Values())
                .WithIdentity(Identities.Empty)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task WithRequestBuilderAndSpecificScheme()
        {
            // You can enforce a custom scheme. In case you have special logic for that scheme in your application
            var response = await _fixture.Server
                .CreateHttpApiRequest<ValuesController>(controller => controller.ValuesWithSchema())
                .WithIdentity(Identities.User, "Bearer")
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }

        [Fact]
        public async Task WithRequestBuilderAndSpecificScheme_Unauthorized()
        {
            var response = await _fixture.Server
                .CreateHttpApiRequest<ValuesController>(controller => controller.ValuesWithSchema())
                .WithIdentity(Identities.User)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Anonymous()
        {
            var response = await _fixture.Server
                .CreateHttpApiRequest<ValuesController>(controller => controller.PublicValues())
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }
    }
}
