using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UnitTests.Acheve.TestHost.Builders;
using UnitTests.Acheve.TestHost.Routing.Models;
using Xunit;

namespace UnitTests.Acheve.TestHost.Routing;

public class CreateApiRequestShould
{
    public const string BASE_PATH = "api/";
    public const string BASE_PATH_BUGS = BASE_PATH + "bugs";
    public const string BASE_PATH_VALUES = BASE_PATH + "values";

    [Fact]
    public void Throw_when_controller_is_not_a_valid_controller()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        Assert.Throws<InvalidOperationException>(() =>
        {
            server.CreateHttpApiRequest<PrivateNonControllerClass>(controller => controller.SomeAction());
        });
        Assert.Throws<InvalidOperationException>(() =>
        {
            server.CreateHttpApiRequest<PublicNonControllerClass>(controller => controller.SomeAction());
        });
        Assert.Throws<InvalidOperationException>(() =>
        {
            server.CreateHttpApiRequest<WithNonControllerAttributeNonControllerClass>(controller => controller.SomeAction());
        });
        Assert.Throws<InvalidOperationException>(() =>
        {
            server.CreateHttpApiRequest<AbstractNonControllerClass>(controller => controller.SomeAction());
        });
        Assert.Throws<InvalidOperationException>(() =>
        {
            server.CreateHttpApiRequest<WithInvalidSufixNonControllerClass>(controller => controller.SomeAction());
        });
    }

    [Fact]
    public void Throw_when_the_action_selector_is_null()
    {
        var server = new TestServerBuilder()
          .UseDefaultStartup()
          .Build();

        Assert.Throws<ArgumentNullException>(() =>
        {
            server.CreateHttpApiRequest<ValuesController>(null);
        });
    }

    [Fact]
    public void Create_valid_request_for_primitive_parameter_action()
    {
        var server = new TestServerBuilder()
          .UseDefaultStartup()
          .Build();

        var request = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Get(0));

        request.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}?id=0");
    }

    [Fact]
    public void Create_valid_request_for_string_as_primitive_parameter_tokenizer_action()
    {
        var server = new TestServerBuilder()
          .UseDefaultStartup()
          .Build();

        var request = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.GetStringAsParameter("unai"));

        request.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/stringasprimitive?value=unai");
    }

    [Fact]
    public void Create_valid_request_for_string_as_primitive_parameter_tokenizer_action_with_case()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var request = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.GetStringAsParameter("Uppercase"));

        request.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/stringasprimitive?value=Uppercase");
    }

    [Fact]
    public void Create_valid_request_for_string_as_decimal_parameter_tokenizer_action()
    {
        var server = new TestServerBuilder()
          .UseDefaultStartup()
          .Build();

        var request = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.GetDecimalAsParameter(2m));

        request.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/decimalasprimitive?value=2");
    }

    [Fact]
    public void Create_valid_request_for_conventional_action_with_extra_parameters()
    {
        var server = new TestServerBuilder()
          .UseDefaultStartup()
          .Build();

        var request = server.CreateHttpApiRequest<ValuesV2Controller>(
            controller => controller.Get(0), new { Version = "v1" });

        request.GetConfiguredAddress()
            .Should().Be("api/v1/values?id=0");
    }

    [Fact]
    public void Create_valid_request_using_verbs_templates()
    {
        var server = new TestServerBuilder()
          .UseDefaultStartup()
          .Build();

        var requestGet = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Get2(0));

        var requestPut = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Put2(1));

        var requestPost = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Post2(2));

        var requestDelete = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Delete2(3));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname?id=0");

        requestPut.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname?id=1");

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname?id=2");

        requestDelete.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname?id=3");
    }

    [Fact]
    public void Create_valid_request_using_verbs_and_parameters()
    {
        var server = new TestServerBuilder()
          .UseDefaultStartup()
          .Build();

        var requestGet = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Get3(0));

        var requestPut = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Put3(1));

        var requestPost = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Post3(2));

        var requestDelete = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Delete3(3));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/0");

        requestPut.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/1");

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/2");

        requestDelete.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/3");
    }

    [Fact]
    public void Create_valid_request_using_verbs_and_extra_parameters()
    {
        var server = new TestServerBuilder()
          .UseDefaultStartup()
          .Build();

        var requestGet = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Get4(0), new { version = "v1" });

        var requestPut = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Put4(1), new { version = "v1" });

        var requestPost = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Post4(2), new { version = "v1" });

        var requestDelete = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Delete4(3), new { version = "v1" });

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/v1/0");

        requestPut.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/v1/1");

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/v1/2");

        requestDelete.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/v1/3");
    }

    [Fact]
    public void Create_valid_request_using_verbs_and_extra_parameters_with_different_case()
    {
        var server = new TestServerBuilder()
          .UseDefaultStartup()
          .Build();

        var requestGet = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Get4(0), new { Version = "v1" });

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/v1/0");
    }

    [Fact]
    public void Create_valid_request_using_route_templates()
    {
        var server = new TestServerBuilder()
          .UseDefaultStartup()
          .Build();

        var requestGet = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Get5(0));

        var requestPut = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Put5(0));

        var requestPost = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Post5(0));

        var requestDelete = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Delete5(0));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overrideroutetemplatemethodname?id=0");

        requestPut.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overrideroutetemplatemethodname?id=0");

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overrideroutetemplatemethodname?id=0");

        requestDelete.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overrideroutetemplatemethodname?id=0");
    }

    [Fact]
    public void Create_valid_request_using_route_templates_with_parameters_and_extra_parameters()
    {
        var server = new TestServerBuilder()
          .UseDefaultStartup()
          .Build();

        var requestGet = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Get6(0), new { version = "v1" });

        var requestPut = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Put6(1), new { version = "v1" });

        var requestPost = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Post6(2), new { version = "v1" });

        var requestDelete = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Delete6(3), new { version = "v1" });

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overrideroutetemplatemethodname/v1/0");

        requestPut.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overrideroutetemplatemethodname/v1/1");

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overrideroutetemplatemethodname/v1/2");

        requestDelete.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overrideroutetemplatemethodname/v1/3");
    }

    [Fact]
    public void Create_valid_request_using_route_templates_and_parameters_with_route_constraints()
    {
        var server = new TestServerBuilder()
          .UseDefaultStartup()
          .Build();

        var requestGet = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Get7(0));

        var requestPut = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Put7(1));

        var requestPost = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Post7(2));

        var requestDelete = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Delete7(3));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overrideroutetemplatemethodname/0");

        requestPut.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overrideroutetemplatemethodname/1");

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overrideroutetemplatemethodname/2");

        requestDelete.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overrideroutetemplatemethodname/3");
    }

    [Fact]
    public void Create_valid_request_using_verbs_and_parameters_with_route_constraints()
    {
        var server = new TestServerBuilder()
         .UseDefaultStartup()
         .Build();

        var requestGet = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Get8(0));

        var requestPut = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Put8(1));

        var requestPost = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Post8(2));

        var requestDelete = server.CreateHttpApiRequest<ValuesController>(
            controller => controller.Delete8(3));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/0");

        requestPut.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/1");

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/2");

        requestDelete.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/overridemethodname/3");
    }

    [Fact]
    public void Create_valid_request_using_verbs_when_parameters_are_not_primitives()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var requestGet = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Get2(complexParameter));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/get2/1/10");
    }

    [Fact]
    public void Create_valid_request_using_verbs_when_parameters_are_not_primitives_with_null_properties()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var requestGet = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Get2(complexParameter));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/get2/1");
    }

    [Fact]
    public void Create_valid_request_using_verbs_and_query_string_when_parameters_are_not_primitives()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var requestGet = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Get1(complexParameter));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/get1?pageindex=1&pagecount=10");
    }

    [Fact]
    public void Create_valid_request_using_verbs_and_query_string_when_parameters_are_not_primitives_with_null_properties()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var requestGet = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Get1(complexParameter));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/get1?pageindex=1");
    }

    [Fact]
    public void Create_valid_request_using_route_when_parameters_are_not_primitives()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var requestGet = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Get4(complexParameter));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/get4/1/10");
    }

    [Fact]
    public void Create_valid_request_using_verbs_query_string_and_from_header()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var header = "HeaderCustom";
        var numberHeader = 1;

        var request = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Get5(header, complexParameter));

        var requestMultipleHeader = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Get6(header, numberHeader, complexParameter));

        var requestOnlyHeader = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Get7(header));

        request.GetRequest().Headers.GetValues("custom").First().Should().Be(header);
        request.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/get5?pageindex=1&pagecount=10");

        requestMultipleHeader.GetRequest().Headers.GetValues("custom1").First().Should().Be(header);
        requestMultipleHeader.GetRequest().Headers.GetValues("custom2").First().Should().Be(numberHeader.ToString());
        requestMultipleHeader.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/get6?pageindex=1&pagecount=10");

        requestOnlyHeader.GetRequest().Headers.GetValues("custom").First().Should().Be(header);
        requestOnlyHeader.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/get7");
    }

    [Fact]
    public void Create_valid_request_using_route_when_parameters_are_not_primitives_with_null_properties()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var requestGet = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Get4(complexParameter));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/get4/1");
    }

    [Fact]
    public void Create_valid_request_using_route_and_query_string_when_parameters_are_not_primitives()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var requestGet = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Get3(complexParameter));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/get3?pageindex=1&pagecount=10");
    }

    [Fact]
    public void Create_valid_request_using_route_and_query_string_when_parameters_are_not_primitives_with_null_properties()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var requestGet = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Get3(complexParameter));

        requestGet.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/get3?pageindex=1");
    }

    [Fact]
    public void Create_valid_request_using_from_body_complex_arguments()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var requestPost = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post1(complexParameter));

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/");
    }

    [Fact]
    public void Create_valid_request_using_from_body_complex_arguments_with_null_properties()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var requestPost = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post1(complexParameter));

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/");
    }

    [Fact]
    public void Create_valid_request_using_from_form_complex_arguments()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var requestPost = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post3(complexParameter),
            tokenValues: null,
            new IncludeContentAsFormUrlEncoded());

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/post3");
    }

    [Fact]
    public void Create_valid_request_using_from_form_complex_arguments_with_null_properties()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var requestPost = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post3(complexParameter),
            tokenValues: null,
            new IncludeContentAsFormUrlEncoded());

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/post3");
    }

    [Fact]
    public void Create_valid_request_using_from_body_complex_arguments_and_primitive_query_parameters()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var requestPost = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post2(2, complexParameter));

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/post/2");
    }

    [Fact]
    public void Create_valid_request_using_from_body_complex_arguments_and_primitive_query_parameters_with_null_properties()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var requestPost = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post2(2, complexParameter));

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/post/2");
    }

    [Fact]
    public void Create_valid_request_using_from_form_complex_arguments_and_primitive_query_parameters()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var requestPost = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post4(2, complexParameter),
            tokenValues: null,
            new IncludeContentAsFormUrlEncoded());

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/post4/2");
    }

    [Fact]
    public void Create_valid_request_using_from_header_primitive_arguments()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var header = "HeaderCustom";

        var requestPost = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post5(header));
        requestPost.GetRequest().Headers.GetValues("custom").First().Should().Be(header);
        requestPost.GetConfiguredAddress()
           .Should().Be($"{BASE_PATH_VALUES}/post5");
    }

    [Fact]
    public void Create_valid_request_using_from_header_primitive_arguments_and_from_body_complex_arguments()
    {
        var server = new TestServerBuilder()
       .UseDefaultStartup()
       .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };
        var header = "HeaderCustom";

        var requestPost2 = server.CreateHttpApiRequest<ValuesV3Controller>(
           controller => controller.Post6(header, complexParameter));

        requestPost2.GetRequest().Headers.GetValues("custom").First().Should().Be(header);
        requestPost2.GetConfiguredAddress().Should().Be($"{BASE_PATH_VALUES}/post6");
    }

    [Fact]
    public void Create_valid_request_using_from_form_complex_arguments_and_primitive_query_parameters_wit_null_properties()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var requestPost = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post4(2, complexParameter),
            tokenValues: null,
            new IncludeContentAsFormUrlEncoded());

        requestPost.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_VALUES}/post4/2");
    }

    [Fact]
    public void Create_valid_request_when_action_use_tilde_to_override_controller_route()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Get1(1));

        requestPost.GetConfiguredAddress()
            .Should().Be("get1/1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Post1(2));

        requestPost.GetConfiguredAddress()
            .Should().Be("post1/2");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
          controller => controller.Put1(3));

        requestPost.GetConfiguredAddress()
            .Should().Be("put1/3");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
          controller => controller.Delete1(4));

        requestPost.GetConfiguredAddress()
            .Should().Be("delete1/4");

        var pagination = new Pagination()
        {
            PageIndex = 1,
            PageCount = 2
        };

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Get2(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("get2/1?pageindex=1&pagecount=2");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Post2(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("post2/1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Put2(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("put2/1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
            controller => controller.Delete2(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("delete2/1?pageindex=1&pagecount=2");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Get3(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("get3/1?pageindex=1&pagecount=2");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Post3(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("post3/1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Put3(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("put3/1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
            controller => controller.Delete3(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("delete3/1?pageindex=1&pagecount=2");
    }

    [Fact]
    public void Create_valid_request_when_action_use_tilde_to_override_controller_route_with_null_properties()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Get1(1));

        requestPost.GetConfiguredAddress()
            .Should().Be("get1/1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Post1(2));

        requestPost.GetConfiguredAddress()
            .Should().Be("post1/2");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
          controller => controller.Put1(3));

        requestPost.GetConfiguredAddress()
            .Should().Be("put1/3");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
          controller => controller.Delete1(4));

        requestPost.GetConfiguredAddress()
            .Should().Be("delete1/4");

        var pagination = new Pagination()
        {
            PageIndex = 1
        };

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Get2(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("get2/1?pageindex=1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Post2(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("post2/1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Put2(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("put2/1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
            controller => controller.Delete2(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("delete2/1?pageindex=1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Get3(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("get3/1?pageindex=1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Post3(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("post3/1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
           controller => controller.Put3(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("put3/1");

        requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
            controller => controller.Delete3(1, pagination));

        requestPost.GetConfiguredAddress()
            .Should().Be("delete3/1?pageindex=1");
    }

    [Fact]
    public async Task Create_request_including_fromBody_argument_as_content_as_default_behavior()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var request = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post2(2, complexParameter),
            tokenValues: null,
            new IncludeContentAsJson());

        var response = await request.PostAsync();

        await response.IsSuccessStatusCodeOrThrow();
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Create_request_including_fromBody_argument_as_content_as_default_behavior_with_null_properties()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var request = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post2(2, complexParameter),
            tokenValues: null,
            new IncludeContentAsJson());

        var response = await request.PostAsync();

        await response.IsSuccessStatusCodeOrThrow();
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Create_request_including_fromBody_argument_as_content_configured_explicitly()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var request = server.CreateHttpApiRequest<ValuesV3Controller>(
            actionSelector: controller => controller.Post2(2, complexParameter),
            tokenValues: null,
            contentOptions: new IncludeContentAsJson());

        var response = await request.PostAsync();

        await response.IsSuccessStatusCodeOrThrow();
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Create_request_including_fromBody_argument_as_content_configured_explicitly_with_null_properties()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var request = server.CreateHttpApiRequest<ValuesV3Controller>(
            actionSelector: controller => controller.Post2(2, complexParameter),
            tokenValues: null,
            contentOptions: new IncludeContentAsJson());

        var response = await request.PostAsync();

        await response.IsSuccessStatusCodeOrThrow();
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Create_request_including_fromForm_argument_as_content_as_default_behavior()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var request = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post4(2, complexParameter),
            tokenValues: null,
            new IncludeContentAsFormUrlEncoded());

        var response = await request.PostAsync();

        await response.IsSuccessStatusCodeOrThrow();
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Create_request_including_fromForm_argument_as_content_as_default_behavior_with_null_properties()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var request = server.CreateHttpApiRequest<ValuesV3Controller>(
            controller => controller.Post4(2, complexParameter),
            tokenValues: null,
            new IncludeContentAsFormUrlEncoded());

        var response = await request.PostAsync();

        await response.IsSuccessStatusCodeOrThrow();
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Create_request_including_fromForm_argument_as_content_configured_explicitly()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var request = server.CreateHttpApiRequest<ValuesV3Controller>(
            actionSelector: controller => controller.Post4(2, complexParameter),
            tokenValues: null,
            contentOptions: new IncludeContentAsFormUrlEncoded());

        var response = await request.PostAsync();

        await response.IsSuccessStatusCodeOrThrow();
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Create_request_including_fromForm_argument_as_content_configured_explicitly_with_null_properties()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var request = server.CreateHttpApiRequest<ValuesV3Controller>(
            actionSelector: controller => controller.Post4(2, complexParameter),
            tokenValues: null,
            contentOptions: new IncludeContentAsFormUrlEncoded());

        var response = await request.PostAsync();

        await response.IsSuccessStatusCodeOrThrow();
        response.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Create_request_including_fromForm_argument_as_content_with_complex_object()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexObject = new ComplexObject()
        {
            BoolNullableParameter = true,
            BoolParameter = true,
            ComplexParameter = new Complex()
            {
                Pagination = new Pagination()
                {
                    PageCount = 10,
                    PageIndex = 1
                },
                LongNullableParameter = 1,
                LongParameter = 1
            },
            IntNullableParameter = 1,
            IntParameter = 1,
            StringParameter = "Test",
            DateTimeParameter = DateTime.Now
        };

        var request = server.CreateHttpApiRequest<RequestContentController>(
            controller => controller.Post(complexObject),
            tokenValues: null,
            new IncludeContentAsFormUrlEncoded());

        var response = await request.PostAsync();

        await response.IsSuccessStatusCodeOrThrow();
    }

    [Fact]
    public async Task Create_request_including_fromForm_argument_as_content_with_complex_object_with_null_properties()
    {
        var server = new TestServerBuilder()
        .UseDefaultStartup()
        .Build();

        var complexObject = new ComplexObject()
        {
            BoolNullableParameter = true,
            BoolParameter = true,
            ComplexParameter = new Complex()
            {
                Pagination = new Pagination()
                {
                    PageIndex = 1
                },
                LongNullableParameter = 1,
                LongParameter = 1
            },
            IntNullableParameter = 1,
            IntParameter = 1,
            StringParameter = "Test",
            DateTimeParameter = DateTime.Now
        };

        var request = server.CreateHttpApiRequest<RequestContentController>(
            controller => controller.Post(complexObject),
            tokenValues: null,
            new IncludeContentAsFormUrlEncoded());

        var response = await request.PostAsync();

        await response.IsSuccessStatusCodeOrThrow();
    }

    [Fact]
    public async Task Create_request_not_adding_fromBody_argument_as_content()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var request = server.CreateHttpApiRequest<ValuesV3Controller>(
            actionSelector: controller => controller.Post2(2, complexParameter),
            tokenValues: null,
            contentOptions: new NotIncludeContent());

        var response = await request.PostAsync();

        response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
    }

    [Fact]
    public async Task Create_request_not_adding_fromBody_argument_as_content_with_null_properties()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var complexParameter = new Pagination()
        {
            PageIndex = 1
        };

        var request = server.CreateHttpApiRequest<ValuesV3Controller>(
            actionSelector: controller => controller.Post2(2, complexParameter),
            tokenValues: null,
            contentOptions: new NotIncludeContent());

        var response = await request.PostAsync();

        response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
    }

    [Fact]
    public void Create_request_supporting_guid_types_on_parameters_and_numbes_on_parameters_names()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var guidValue = Guid.NewGuid();

        var request = server.CreateHttpApiRequest<BugsController>(
            actionSelector: controller => controller.GuidSupport("prm1", guidValue),
            tokenValues: null,
            contentOptions: new NotIncludeContent());

        request.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_BUGS}/prm1/{guidValue}");
    }

    [Fact]
    public void Create_valid_request_without_using_frombody_with_apicontroller_attribute()
    {
        var server = new TestServerBuilder().UseDefaultStartup()
                                            .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var requestPost1 = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.Post1(complexParameter));

        string body = requestPost1.GetRequest().Content.ReadAsStringAsync().Result;
        JsonSerializer.Deserialize<Pagination>(body).PageIndex.Should().Be(complexParameter.PageIndex);
        JsonSerializer.Deserialize<Pagination>(body).PageCount.Should().Be(complexParameter.PageCount);
    }

    [Fact]
    public void Create_valid_request_without_using_frombody_with_apicontroller_attribute_and_route_parameter()
    {
        var server = new TestServerBuilder().UseDefaultStartup()
                                            .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var requestPost2 = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.Post2(1, complexParameter));

        string body = requestPost2.GetRequest().Content.ReadAsStringAsync().Result;
        JsonSerializer.Deserialize<Pagination>(body).PageIndex.Should().Be(complexParameter.PageIndex);
        JsonSerializer.Deserialize<Pagination>(body).PageCount.Should().Be(complexParameter.PageCount);
        requestPost2.GetConfiguredAddress().StartsWith($"{BASE_PATH_VALUES}/1").Should().Be(true);
    }

    [Fact]
    public void Create_valid_request_without_using_frombody_with_apicontroller_and_string_parameter_in_route()
    {
        var server = new TestServerBuilder().UseDefaultStartup()
                                            .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        string id = "A1";

        var requestPost3 = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.Post3($"{id}", complexParameter));

        string body = requestPost3.GetRequest().Content.ReadAsStringAsync().Result;
        JsonSerializer.Deserialize<Pagination>(body).PageIndex.Should().Be(complexParameter.PageIndex);
        JsonSerializer.Deserialize<Pagination>(body).PageCount.Should().Be(complexParameter.PageCount);
        requestPost3.GetConfiguredAddress().StartsWith($"{BASE_PATH_VALUES}/{id}").Should().Be(true);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_valid_request_without_using_frombody_with_apicontroller_and_string_parameter_with_invalid_value(string id)
    {
        var server = new TestServerBuilder().UseDefaultStartup()
                                            .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var requestPost3 = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.Post3($"{id}", complexParameter));

        string body = requestPost3.GetRequest().Content.ReadAsStringAsync().Result;
        JsonSerializer.Deserialize<Pagination>(body).PageIndex.Should().Be(complexParameter.PageIndex);
        JsonSerializer.Deserialize<Pagination>(body).PageCount.Should().Be(complexParameter.PageCount);
        requestPost3.GetConfiguredAddress().StartsWith($"{BASE_PATH_VALUES}/").Should().Be(true);
    }

    [Fact]
    public void Create_valid_request_of_patch_without_using_frombody_with_apicontroller_attribute_and_route_parameter()
    {
        var server = new TestServerBuilder().UseDefaultStartup()
                                            .Build();

        var complexParameter = new Pagination()
        {
            PageCount = 10,
            PageIndex = 1
        };

        var requestPost2 = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.Patch1(1, complexParameter));

        string body = requestPost2.GetRequest().Content.ReadAsStringAsync().Result;
        JsonSerializer.Deserialize<Pagination>(body).PageIndex.Should().Be(complexParameter.PageIndex);
        JsonSerializer.Deserialize<Pagination>(body).PageCount.Should().Be(complexParameter.PageCount);
        requestPost2.GetConfiguredAddress().StartsWith($"{BASE_PATH_VALUES}/1").Should().Be(true);
    }

    [Fact]
    public void Create_valid_request_supporting_underdash_on_router_params()
    {
        var server = new TestServerBuilder()
       .UseDefaultStartup()
       .Build();

        var guid = Guid.NewGuid();

        var request = server.CreateHttpApiRequest<BugsController>(
            actionSelector: controller => controller.UnderDashSupport(guid, 10),
            tokenValues: null,
            contentOptions: new NotIncludeContent());

        request.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_BUGS}/{guid}/10");
    }

    [Fact]
    public async Task Create_valid_request_supporting_nullable_params_on_query()
    {
        var server = new TestServerBuilder()
       .UseDefaultStartup()
       .Build();

        var guid = Guid.NewGuid();

        var request = server.CreateHttpApiRequest<BugsController>(
            actionSelector: controller => controller.NullableQueryParams(null, guid),
            tokenValues: null,
            contentOptions: new NotIncludeContent());

        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.ReadContentAsAsync<NullableQueryParamsResponse>();

        response.Param1.Should().Be(null);
        response.Param2.Should().Be(guid);
    }

    [Fact]
    public async Task Create_request_supporting_guid_array_types_on_parameters()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var guidList = new List<Guid> {
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        };

        var array = guidList.ToArray();

        var request = server.CreateHttpApiRequest<BugsController>(
            actionSelector: controller => controller.GuidArraySupport(array),
            tokenValues: null,
            contentOptions: new NotIncludeContent());

        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.ReadContentAsAsync<Guid[]>();

        response.Should().NotBeNull();
        response.Should().HaveCount(3);
    }

    [Fact]
    public async Task Create_request_supporting_int_array_types_on_parameters()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        int[] array = { 1, 3, 5, 7, 9 };

        var request = server.CreateHttpApiRequest<BugsController>(
            actionSelector: controller => controller.IntArraySupport(array),
            tokenValues: null,
            contentOptions: new NotIncludeContent());

        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.ReadContentAsAsync<int[]>();

        response.Should().NotBeNull();
        response.Should().HaveCount(5);
    }

    [Fact]
    public async Task Create_request_not_supporting_class_array_types_on_parameters()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var array = new Person[] {
            new Person { FirstName = "john", LastName = "walter" },
            new Person { FirstName = "john2", LastName = "walter2" }
        };

        var request = server.CreateHttpApiRequest<BugsController>(
            actionSelector: controller => controller.PersonArraySupport(array),
            tokenValues: null,
            contentOptions: new NotIncludeContent());

        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.ReadContentAsAsync<Person[]>();

        response.Should().NotBeNull();
        response.Should().HaveCount(0);
    }

    [Fact]
    public async Task Create_request_supporting_string_array_types_on_parameters()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        string[] array = { "one", "two", "three" };

        var request = server.CreateHttpApiRequest<BugsController>(
            actionSelector: controller => controller.StringArraySupport(array),
            tokenValues: null,
            contentOptions: new NotIncludeContent());

        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.ReadContentAsAsync<string[]>();

        response.Should().NotBeNull();
        response.Should().HaveCount(3);
    }

    [Fact]
    public async Task Create_request_supporting_guid_array_types_on_parameters_seding_method()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var guidList = new List<Guid> {
        Guid.NewGuid(),
        Guid.NewGuid(),
        Guid.NewGuid(),
        };

        var request = server.CreateHttpApiRequest<BugsController>(
            actionSelector: controller => controller.GuidArraySupport(guidList.ToArray()),
            tokenValues: null,
            contentOptions: new NotIncludeContent());

        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.ReadContentAsAsync<Guid[]>();

        response.Should().NotBeNull();
        response.Should().HaveCount(3);
    }

    [Fact]
    public void Create_request_supporting_send_method_on_client_http()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var guid = Guid.NewGuid().ToString();

        var request = server.CreateHttpApiRequest<BugsController>(
            actionSelector: controller => controller.GuidSupport("prm1", Guid.Parse(guid)),
            tokenValues: null,
            contentOptions: new NotIncludeContent());

        request.GetConfiguredAddress()
            .Should().Be($"{BASE_PATH_BUGS}/prm1/{guid}");
    }

    [Fact]
    public async Task Create_request_supporting_router_and_body_params()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var guid = Guid.NewGuid();
        var person = new Person { FirstName = "john", LastName = "walter" };

        var request = server.CreateHttpApiRequest<BugsController>(
            actionSelector: controller => controller.AllowRouterAndBodyParams(guid, person),
            tokenValues: null);

        var responseMessage = await request.PostAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.ReadContentAsAsync<RouterAndBodyParamsResponse>();

        response.Should().NotBeNull();
        response.TestId.Should().Be(guid);
        response.Person.Should().NotBeNull();
        response.Person.FirstName.Should().Be(person.FirstName);
        response.Person.LastName.Should().Be(person.LastName);
    }

    [Fact]
    public async Task Create_request_supporting_template_with_serveral_colon()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();
        const int param1 = 1;
        const int param2 = 2;

        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.GetWithSeveralColon(param1, param2));

        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.Content.ReadAsStringAsync();

        response.Should().NotBeNull().And.Be($"{param1}/{param2}");
    }

    [Fact]
    public async Task Create_request_from_implemented_abstract_method()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var request = server.CreateHttpApiRequest<ImplementationAbstractController>(controller => controller.AbstractMethod());

        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        responseMessage.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Create_request_from_overrided_virtual_method()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var request = server.CreateHttpApiRequest<ImplementationAbstractController>(controller => controller.VirtualMethod());

        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        var content = await responseMessage.Content.ReadAsStringAsync();
        content.Should().Be(ImplementationAbstractController.VIRTUAL_METHOD_RESULT);
    }

    [Fact]
    public async Task Create_request_from_not_overrided_virtual_method()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var request = server.CreateHttpApiRequest<ImplementationAbstractController>(controller => controller.Virtual2Method());

        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        responseMessage.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Create_get_request_with_cancellation_token_parameter()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var param = "one";
        using var source = new CancellationTokenSource();
        var token = source.Token;

        var request = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.GetWithCancellationToken(param, token));
        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.Content.ReadAsStringAsync();
        response.Should().Be(param);
    }

    [Fact]
    public async Task Create_post_request_with_cancellation_token_parameter()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var param = "one";
        using var source = new CancellationTokenSource();
        var token = source.Token;

        var request = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.PostWithCancellationToken(param, token));
        var responseMessage = await request.PostAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.Content.ReadAsStringAsync();
        response.Should().Be(param);
    }

    [Fact]
    public async Task Create_request_with_action_name_in_route()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.ActionNameInRoute());

        request.GetConfiguredAddress().Should().Be($"{BASE_PATH_BUGS}/{nameof(BugsController.ActionNameInRoute)}".ToLower());

        var responseMessage = await request.GetAsync();
        responseMessage.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Create_get_request_with_string_list_from_query()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var param = ParamWithSeveralTypes.CreateRandom().StringValues;

        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.GetWithListParam(param));

        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.ReadContentAsAsync<IEnumerable<string>>();

        response.Should().BeEquivalentTo(param);
    }

    [Fact]
    public async Task Create_post_request_with_string_list_from_body()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = ParamWithSeveralTypes.CreateRandom().StringValues;

        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.PostWithListParam(model));
        var responseMessage = await request.PostAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.ReadContentAsAsync<IEnumerable<string>>();
        response.Should().Equal(model);
    }

    [Fact]
    public async Task Create_get_request_with_datetime_from_query()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = ParamWithSeveralTypes.CreateRandom().DateTimeValue;

        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.GetWithDatetimeParam(model));
        var responseMessage = await request.GetAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.ReadContentAsAsync<DateTime>();
        response.Should().Be(model);
    }

    [Fact]
    public async Task Create_post_request_with_datetime_from_body()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = ParamWithSeveralTypes.CreateRandom().DateTimeValue;

        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.PostWithDatetimeParam(model));
        var responseMessage = await request.PostAsync();

        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.ReadContentAsAsync<DateTime>();
        response.Should().Be(model);
    }

    [Fact]
    public async Task Create_get_request_with_datetime_list_from_query()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = ParamWithSeveralTypes.CreateRandom().DateTimeValues;

        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.GetWithDatetimeListParam(model));
        var responseMessage = await request.GetAsync();

        await responseMessage.IsSuccessStatusCodeOrThrow();
        var response = await responseMessage.ReadContentAsAsync<List<DateTime>>();
        response.Should().Equal(model);
    }

    [Fact]
    public async Task Create_post_request_with_datetime_list_from_body()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = ParamWithSeveralTypes.CreateRandom().DateTimeValues;

        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.PostWithDatetimeListParam(model));
        var responseMessage = await request.PostAsync();

        await responseMessage.IsSuccessStatusCodeOrThrow();
        var response = await responseMessage.ReadContentAsAsync<List<DateTime>>();
        response.Should().Equal(model);
    }

    [Fact]
    public async Task Create_get_request_with_object_from_query()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = ParamWithSeveralTypes.CreateRandom();

        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.GetWithObject(model));
        var responseMessage = await request.GetAsync();

        await responseMessage.IsSuccessStatusCodeOrThrow();
        var response = await responseMessage.ReadContentAsAsync<ParamWithSeveralTypes>();
        response.Should().Be(model);
    }

    [Fact]
    public async Task Create_post_request_with_object_from_body()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = ParamWithSeveralTypes.CreateRandom();

        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.PostWithObject(model));
        var responseMessage = await request.PostAsync();

        await responseMessage.IsSuccessStatusCodeOrThrow();
        var response = await responseMessage.ReadContentAsAsync<ParamWithSeveralTypes>();
        response.Should().Be(model);
    }

    [Fact]
    public async Task Create_post_request_with_object_from_form()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = ParamWithSeveralTypes.CreateRandom();

        var request = server.CreateHttpApiRequest<BugsController>(controller => controller.PostWithObjectFromForm(model));
        var responseMessage = await request.PostAsync();

        await responseMessage.IsSuccessStatusCodeOrThrow();
        var response = await responseMessage.ReadContentAsAsync<ParamWithSeveralTypes>();
        response.Should().Be(model);
    }

    [Fact]
    public async Task Create_get_request_with_object_from_query_setting_action_response_type()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = ParamWithSeveralTypes.CreateRandom();

        var request = server.CreateHttpApiRequest<BugsController, ActionResult<ParamWithSeveralTypes>>(
            controller => controller.GetWithObject(model)
        );
        var responseMessage = await request.GetAsync();

        await responseMessage.IsSuccessStatusCodeOrThrow();
        var response = await responseMessage.ReadContentAsAsync<ParamWithSeveralTypes>();
        response.Should().Be(model);
    }

    [Fact]
    public async Task Create_post_request_with_file()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = server.GivenFile(content: nameof(Create_post_request_with_file));
        var request = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.PostFile(model));
        var responseMessage = await request.PostAsync();

        await responseMessage.IsSuccessStatusCodeOrThrow();
        var content = await responseMessage.Content.ReadAsStringAsync();
        content.Should().Be(nameof(Create_post_request_with_file));
    }

    [Fact]
    public async Task Create_post_request_with_object_with_file()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var file = server.GivenFile(content: nameof(Create_post_request_with_file));
        var model = new ParamWithFile(file);
        var request = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.PostObjectWithFile(model));
        var responseMessage = await request.PostAsync();

        await responseMessage.IsSuccessStatusCodeOrThrow();
        var content = await responseMessage.Content.ReadAsStringAsync();
        content.Should().Be($"{model.Id}+{nameof(Create_post_request_with_file)}");
    }

    [Fact]
    public async Task Create_post_request_with_object_with_different_froms()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = new ParamWithDifferentFroms()
        {
            ParamFromBody = ParamWithSeveralTypes.CreateRandom(),
            ParamFromQuery = "fromQuery",
            ParamFromRoute = "fromRoute",
            ParamFromHeader = "fromHeader"
        };
        var request = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.PostWithDifferentFroms(model));
        var responseMessage = await request.PostAsync();

        await responseMessage.IsSuccessStatusCodeOrThrow();
        var content = await responseMessage.ReadContentAsAsync<ParamWithDifferentFroms>();
        content.ParamFromHeader.Should().Be(model.ParamFromHeader);
        content.ParamFromQuery.Should().Be(model.ParamFromQuery);
        content.ParamFromRoute.Should().Be(model.ParamFromRoute);
        content.ParamFromBody.Should().Be(model.ParamFromBody);
    }

    [Fact]
    public async Task Create_put_request_with_object_with_different_froms()
    {
        var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

        var model = new ParamWithDifferentFroms()
        {
            ParamFromBody = ParamWithSeveralTypes.CreateRandom(),
            ParamFromQuery = "fromQuery",
            ParamFromRoute = "fromRoute",
            ParamFromHeader = "fromHeader"
        };
        var request = server.CreateHttpApiRequest<ValuesV5Controller>(controller => controller.PutWithDifferentFroms(model));
        var responseMessage = await request.SendAsync("PUT");

        await responseMessage.IsSuccessStatusCodeOrThrow();
        var content = await responseMessage.ReadContentAsAsync<ParamWithDifferentFroms>();
        content.ParamFromHeader.Should().Be(model.ParamFromHeader);
        content.ParamFromQuery.Should().Be(model.ParamFromQuery);
        content.ParamFromRoute.Should().Be(model.ParamFromRoute);
        content.ParamFromBody.Should().Be(model.ParamFromBody);
    }

    private class PrivateNonControllerClass
    {
        public int SomeAction()
        {
            return 0;
        }
    }

    public class PublicNonControllerClass
    {
        public int SomeAction()
        {
            return 0;
        }
    }

    [NonController()]
    public class WithNonControllerAttributeNonControllerClass
    {
        public int SomeAction()
        {
            return 0;
        }
    }

    public class AbstractNonControllerClass
    {
        public int SomeAction()
        {
            return 0;
        }
    }

    public class WithInvalidSufixNonControllerClass
    {
        public int SomeAction()
        {
            return 0;
        }
    }
}