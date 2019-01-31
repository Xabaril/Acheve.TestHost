using System.Linq;
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
    [Collection(Collections.Api)]
    public class VauesWithDefaultUserTests
    {
        private readonly TestHostFixture _fixture;

        public VauesWithDefaultUserTests(TestHostFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Authorized_User_Should_Get_200()
        {
            var response = await _fixture.Server.CreateHttpApiRequest<ValuesController>(controller=>controller.Values())
                .WithIdentity(Identities.User)
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }

        [Fact]
        public async Task User_With_No_Claims_Is_Forbidden()
        {
            var response = await _fixture.Server.CreateHttpApiRequest<ValuesController>(controller => controller.Values())
                .WithIdentity(Identities.Empty)
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Authorized_User_Should_Get_200_Using_A_Specific_Scheme()
        {
            var response = await _fixture.Server.CreateHttpApiRequest<ValuesController>(controller => controller.ValuesWithSchema())
                .WithIdentity(Identities.User, "Bearer")
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }

        [Fact]
        public async Task WithRequestBuilderAndSpecificSchemeUnauthorized()
        {
            var response = await _fixture.Server.CreateHttpApiRequest<ValuesController>(controller => controller.ValuesWithSchema())
                .WithIdentity(Identities.User) // We are not using the expected "Bearer" schema
                .GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            response.Headers.WwwAuthenticate.Count.Should().Be(1);
            response.Headers.WwwAuthenticate.First().Scheme.Should().Be("Bearer");
        }

        [Fact]
        public async Task Authentication_Is_Not_Performed_For_Non_Protected_Endpoints()
        {
            var response = await _fixture.Server.CreateHttpApiRequest<ValuesController>(controller => controller.PublicValues())
                .GetAsync();

            await response.IsSuccessStatusCodeOrThrow();
        }
    }
}
