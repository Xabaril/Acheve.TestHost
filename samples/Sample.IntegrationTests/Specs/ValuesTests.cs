using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Sample.Api.Controllers;
using Sample.IntegrationTests.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sample.IntegrationTests.Specs
{
    public abstract class ValuesWithDefaultUserTests
    {
        private readonly TestServer _server;

        protected ValuesWithDefaultUserTests(TestServer server) => _server = server;

        [Fact]
        public async Task Authorized_User_Should_Get_200()
        {
            var response = await _server.CreateHttpApiRequest<ValuesController>(controller => controller.Values())
                .WithIdentity(Identities.User)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task User_With_No_Claims_Is_Forbidden()
        {
            var response = await _server.CreateHttpApiRequest<ValuesController>(controller => controller.Values())
                .WithIdentity(Identities.Empty)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Authorized_User_Should_Get_200_Using_A_Specific_Scheme()
        {
            var response = await _server.CreateHttpApiRequest<ValuesController>(controller => controller.ValuesWithSchema())
                .WithIdentity(Identities.User, "Bearer")
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task WithRequestBuilderAndSpecificSchemeUnauthorized()
        {
            var response = await _server.CreateHttpApiRequest<ValuesController>(controller => controller.ValuesWithSchema())
                .WithIdentity(Identities.User) // We are not using the expected "Bearer" schema
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            response.Headers.WwwAuthenticate.Count.Should().Be(1);
            response.Headers.WwwAuthenticate.First().Scheme.Should().Be("Bearer");
        }

        [Fact]
        public async Task Authentication_Is_Not_Performed_For_Non_Protected_Endpoints()
        {
            var response = await _server.CreateHttpApiRequest<ValuesController>(controller => controller.PublicValues())
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task WithRequestBuilderAndNullParameter()
        {
            var response = await _server.CreateHttpApiRequest<ValuesController>(controller => controller.ModelValues(null))
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task WithRequestBuilderAndNullPrimitiveParameter()
        {
            var response = await _server.CreateHttpApiRequest<ValuesController>(controller => controller.PrimitiveValues(null))
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
            response.IsSuccessStatusCode.Should().BeTrue();
        }
    }

    [Collection(nameof(ApiCollection))]
    public class ValuesWithDefaultUserTestHostTests : ValuesWithDefaultUserTests
    {
        public ValuesWithDefaultUserTestHostTests(TestHostFixture fixture) : base(fixture.Server) { }

    }

    [Collection(nameof(WebApplicationFactoryApiCollection))]
    public class ValuesWithDefaultUserWebApplicationFactoryTests : ValuesWithDefaultUserTests
    {
        public ValuesWithDefaultUserWebApplicationFactoryTests(WebApplicationFactoryFixture fixture) : base(fixture.Server) { }
    }
}
