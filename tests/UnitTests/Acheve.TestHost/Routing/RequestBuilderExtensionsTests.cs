using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using UnitTests.Acheve.TestHost.Builders;
using Xunit;

namespace UnitTests.Acheve.TestHost.Routing
{
    public class RequestBuilderExtensionsTests
    {
        public const string BASE_PATH = "api/values/";

        [Fact]
        public async Task Create_request_and_add_parameter()
        {
            var server = new TestServerBuilder()
                .UseDefaultStartup()
                .Build();

            var id = new Random().Next(1, 100);

            var request = server.CreateHttpApiRequest<ValuesController>(controller => controller.GetParameterFromRequestQuery())
                .AddQueryParameter(nameof(id), id);

            var responseMessage = await request.GetAsync();

            responseMessage.EnsureSuccessStatusCode();
            var response = await responseMessage.Content.ReadAsStringAsync();

            response.Should().Be(id.ToString());
        }

        [Fact]
        public async Task Create_request_and_add_additional_parameter()
        {
            var server = new TestServerBuilder()
                .UseDefaultStartup()
                .Build();

            var id1 = new Random().Next(1, 100);
            var id2 = new Random().Next(1, 100);

            var request = server.CreateHttpApiRequest<ValuesController>(controller => controller.GetAdditionalParameterFromRequestQuery(id1))
                .AddQueryParameter(nameof(id2), id2);

            var responseMessage = await request.GetAsync();

            responseMessage.EnsureSuccessStatusCode();
            var response = await responseMessage.Content.ReadAsStringAsync();

            response.Should().Be(JsonSerializer.Serialize(new { id1 = id1.ToString(), id2 = id2.ToString() }));
        }

        [Fact]
        public async Task Create_request_and_add_parameter_when_you_have_path_parameter()
        {
            var server = new TestServerBuilder()
                .UseDefaultStartup()
                .Build();

            var id1 = new Random().Next(1, 100);
            var id2 = new Random().Next(1, 100);

            var request = server.CreateHttpApiRequest<ValuesController>(controller => controller.GetAdditionalParameterFromRequestQueryAndPath(id1))
                .AddQueryParameter(nameof(id2), id2);

            var responseMessage = await request.GetAsync();

            responseMessage.EnsureSuccessStatusCode();
            var response = await responseMessage.Content.ReadAsStringAsync();

            response.Should().Be(JsonSerializer.Serialize(new { id1 = id1.ToString(), id2 = id2.ToString() }));
        }

        [Fact]
        public void Remove_parameter_when_you_have_one_parameter()
        {
            var server = new TestServerBuilder()
                .UseDefaultStartup()
                .Build();

            const string PARAMETER_TO_REMOVE = "parameter1";
            const string URL = BASE_PATH + "?" + PARAMETER_TO_REMOVE + "=valueParameter1";

            var requestBuilder = server.CreateRequest(URL)
                .RemoveQueryParameter(PARAMETER_TO_REMOVE);

            var requestUrl = requestBuilder.GetRequest().RequestUri.ToString();

            const string EXPECTED_URL = BASE_PATH;
            requestUrl.Should().Be(EXPECTED_URL);
        }

        [Fact]
        public void Remove_last_parameter_when_you_have_two_parameter()
        {
            var server = new TestServerBuilder()
                .UseDefaultStartup()
                .Build();

            const string PARAMETER_TO_REMOVE = "parameter2";
            const string URL_FIRST_PART = "?parameter1=valueParameter1";
            const string URL = BASE_PATH + URL_FIRST_PART + "&" + PARAMETER_TO_REMOVE + "=valueParameter2";

            var requestBuilder = server.CreateRequest(URL)
                .RemoveQueryParameter(PARAMETER_TO_REMOVE);

            var requestUrl = requestBuilder.GetRequest().RequestUri.ToString();

            const string EXPECTED_URL = BASE_PATH + URL_FIRST_PART;
            requestUrl.Should().Be(EXPECTED_URL);
        }

        [Fact]
        public void Remove_first_parameter_when_you_have_two_parameter()
        {
            var server = new TestServerBuilder()
                .UseDefaultStartup()
                .Build();

            const string PARAMETER_TO_REMOVE = "parameter1";
            const string URL_LAST_PART = "parameter2=valueParameter2";
            const string URL = BASE_PATH + "?" + PARAMETER_TO_REMOVE + "=valueParameter1&" + URL_LAST_PART;

            var requestBuilder = server.CreateRequest(URL)
                .RemoveQueryParameter(PARAMETER_TO_REMOVE);

            var requestUrl = requestBuilder.GetRequest().RequestUri.ToString();

            const string EXPECTED_URL = BASE_PATH + "?" + URL_LAST_PART;
            requestUrl.Should().Be(EXPECTED_URL);
        }

        [Theory]
        [InlineData("?parameter1=valueParameter1&", "parameter2=valueParameter2")]
        [InlineData("?parameter1=valueParameter1&", "parameter2=valueParameter2&parameter3=valueParametere")]
        [InlineData("?parameter1=valueParameter1&parameter2=valueParameter2&", "parameter3=valueParameter3")]
        [InlineData("?parameter1=valueParameter1&parameter2=valueParameter2&", "parameter3=valueParameter3&parameter4=valueParameter4")]
        public void Remove_parameter_when_is_beetween_parameters(string urlFirstPart, string urlLastPart)
        {
            var server = new TestServerBuilder()
                .UseDefaultStartup()
                .Build();

            const string PARAMETER_TO_REMOVE = "parameterA";
            string url = BASE_PATH + urlFirstPart + PARAMETER_TO_REMOVE + "=valueParameterA&" + urlLastPart;

            var requestBuilder = server.CreateRequest(url)
                .RemoveQueryParameter(PARAMETER_TO_REMOVE);

            var requestUrl = requestBuilder.GetRequest().RequestUri.ToString();

            string expectedUrl = BASE_PATH + urlFirstPart + urlLastPart;
            requestUrl.Should().Be(expectedUrl);
        }
    }
}
