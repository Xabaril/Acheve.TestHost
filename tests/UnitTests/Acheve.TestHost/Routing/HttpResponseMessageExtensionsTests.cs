using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnitTests.Acheve.TestHost.Builders;
using UnitTests.Acheve.TestHost.Routing.Models;
using Xunit;

namespace UnitTests.Acheve.TestHost.Routing;

public class HttpResponseMessageExtensionsTests
{
    [Fact]
    public async Task IsSuccessStatusCodeOrThrow_throw_exception()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var request = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.GetBadRequest());
        var responseMessage = await request.GetAsync();

        var isSuccessFunc = () => responseMessage.IsSuccessStatusCodeOrThrow();
        await isSuccessFunc.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task IsSuccessStatusCodeOrThrow_not_throw_exception()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var request = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.GetOk());
        var responseMessage = await request.GetAsync();

        var isSuccessFunc = () => responseMessage.IsSuccessStatusCodeOrThrow();
        await isSuccessFunc.Should().NotThrowAsync<Exception>();
    }

    [Fact]
    public async Task ReadContentAsAsync_return_complex_object()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = ParamWithSeveralTypes.CreateRandom();
        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.GetWithObject(model));
        var responseMessage = await request.GetAsync();

        var response = await responseMessage.ReadContentAsAsync<ParamWithSeveralTypes>();

        response.Should().Be(model);
    }

    [Fact]
    public async Task ReadContentAsAsync_return_string()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var param1 = 1;
        var param2 = 1;
        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.GetWithSeveralColon(param1, param2));
        var responseMessage = await request.GetAsync();

        var response = await responseMessage.ReadContentAsAsync<string>();

        response.Should().Be($"{param1}/{param2}");
    }
}
