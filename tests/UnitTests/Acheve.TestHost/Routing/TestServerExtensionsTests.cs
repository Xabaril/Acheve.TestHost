using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnitTests.Acheve.TestHost.Builders;
using Xunit;

namespace UnitTests.Acheve.TestHost.Routing
{
    public class create_api_request_should
    {
        [Fact]
        public void throw_when_controller_is_not_a_valid_controller()
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
        public void throw_when_the_action_selector_is_null()
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
        public void create_valid_request_for_primitive_parameter_action()
        {
            var server = new TestServerBuilder()
              .UseDefaultStartup()
              .Build();

            var request = server.CreateHttpApiRequest<ValuesController>(
                controller => controller.Get(0));

            request.GetConfiguredAddress()
                .Should().Be("api/values?id=0");
        }

        [Fact]
        public void create_valid_request_for_string_as_primitive_parameter_tokenizer_action()
        {
            var server = new TestServerBuilder()
              .UseDefaultStartup()
              .Build();

            var request = server.CreateHttpApiRequest<ValuesController>(
                controller => controller.GetStringAsParameter("unai"));

            request.GetConfiguredAddress()
                .Should().Be("api/values/stringasprimitive?value=unai");
        }

        [Fact]
        public void create_valid_request_for_string_as_primitive_parameter_tokenizer_action_with_case()
        {
            var server = new TestServerBuilder()
                .UseDefaultStartup()
                .Build();

            var request = server.CreateHttpApiRequest<ValuesController>(
                controller => controller.GetStringAsParameter("Uppercase"));

            request.GetConfiguredAddress()
                .Should().Be("api/values/stringasprimitive?value=Uppercase");
        }

        [Fact]
        public void create_valid_request_for_string_as_decimal_parameter_tokenizer_action()
        {
            var server = new TestServerBuilder()
              .UseDefaultStartup()
              .Build();

            var request = server.CreateHttpApiRequest<ValuesController>(
                controller => controller.GetDecimalAsParameter(2m));

            request.GetConfiguredAddress()
                .Should().Be("api/values/decimalasprimitive?value=2");
        }

        [Fact]
        public void create_valid_request_for_conventional_action_with_extra_parameters()
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
        public void create_valid_request_using_verbs_templates()
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
                .Should().Be("api/values/overridemethodname?id=0");

            requestPut.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname?id=1");

            requestPost.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname?id=2");

            requestDelete.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname?id=3");
        }

        [Fact]
        public void create_valid_request_using_verbs_and_parameters()
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
                .Should().Be("api/values/overridemethodname/0");

            requestPut.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname/1");

            requestPost.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname/2");

            requestDelete.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname/3");
        }

        [Fact]
        public void create_valid_request_using_verbs_and_extra_parameters()
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
                .Should().Be("api/values/overridemethodname/v1/0");

            requestPut.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname/v1/1");

            requestPost.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname/v1/2");

            requestDelete.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname/v1/3");
        }

        [Fact]
        public void create_valid_request_using_verbs_and_extra_parameters_with_different_case()
        {
            var server = new TestServerBuilder()
              .UseDefaultStartup()
              .Build();

            var requestGet = server.CreateHttpApiRequest<ValuesController>(
                controller => controller.Get4(0), new { Version = "v1" });

            requestGet.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname/v1/0");
        }

        [Fact]
        public void create_valid_request_using_route_templates()
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
                .Should().Be("api/values/overrideroutetemplatemethodname?id=0");

            requestPut.GetConfiguredAddress()
                .Should().Be("api/values/overrideroutetemplatemethodname?id=0");

            requestPost.GetConfiguredAddress()
                .Should().Be("api/values/overrideroutetemplatemethodname?id=0");

            requestDelete.GetConfiguredAddress()
                .Should().Be("api/values/overrideroutetemplatemethodname?id=0");
        }

        [Fact]
        public void create_valid_request_using_route_templates_with_parameters_and_extra_parameters()
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
                .Should().Be("api/values/overrideroutetemplatemethodname/v1/0");

            requestPut.GetConfiguredAddress()
                .Should().Be("api/values/overrideroutetemplatemethodname/v1/1");

            requestPost.GetConfiguredAddress()
                .Should().Be("api/values/overrideroutetemplatemethodname/v1/2");

            requestDelete.GetConfiguredAddress()
                .Should().Be("api/values/overrideroutetemplatemethodname/v1/3");
        }

        [Fact]
        public void create_valid_request_using_route_templates_and_parameters_with_route_constraints()
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
                .Should().Be("api/values/overrideroutetemplatemethodname/0");

            requestPut.GetConfiguredAddress()
                .Should().Be("api/values/overrideroutetemplatemethodname/1");

            requestPost.GetConfiguredAddress()
                .Should().Be("api/values/overrideroutetemplatemethodname/2");

            requestDelete.GetConfiguredAddress()
                .Should().Be("api/values/overrideroutetemplatemethodname/3");
        }

        [Fact]
        public void create_valid_request_using_verbs_and_parameters_with_route_constraints()
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
                .Should().Be("api/values/overridemethodname/0");

            requestPut.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname/1");

            requestPost.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname/2");

            requestDelete.GetConfiguredAddress()
                .Should().Be("api/values/overridemethodname/3");
        }

        [Fact]
        public void create_valid_request_using_verbs_when_parameters_are_not_primitives()
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
                .Should().Be("api/values/get2/1/10");
        }

        [Fact]
        public void create_valid_request_using_verbs_when_parameters_are_not_primitives_with_null_properties()
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
                .Should().Be("api/values/get2/1");
        }

        [Fact]
        public void create_valid_request_using_verbs_and_query_string_when_parameters_are_not_primitives()
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
                .Should().Be("api/values/get1?pageindex=1&pagecount=10");
        }

        [Fact]
        public void create_valid_request_using_verbs_and_query_string_when_parameters_are_not_primitives_with_null_properties()
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
                .Should().Be("api/values/get1?pageindex=1");
        }

        [Fact]
        public void create_valid_request_using_route_when_parameters_are_not_primitives()
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
                .Should().Be("api/values/get4/1/10");
        }

        [Fact]
        public void create_valid_request_using_verbs_query_string_and_from_header()
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
                .Should().Be("api/values/get5?pageindex=1&pagecount=10");

            requestMultipleHeader.GetRequest().Headers.GetValues("custom1").First().Should().Be(header);
            requestMultipleHeader.GetRequest().Headers.GetValues("custom2").First().Should().Be(numberHeader.ToString());
            requestMultipleHeader.GetConfiguredAddress()
                .Should().Be("api/values/get6?pageindex=1&pagecount=10");

            requestOnlyHeader.GetRequest().Headers.GetValues("custom").First().Should().Be(header);
            requestOnlyHeader.GetConfiguredAddress()
                .Should().Be("api/values/get7");
        }

        [Fact]
        public void create_valid_request_using_route_when_parameters_are_not_primitives_with_null_properties()
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
                .Should().Be("api/values/get4/1");
        }

        [Fact]
        public void create_valid_request_using_route_and_query_string_when_parameters_are_not_primitives()
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
                .Should().Be("api/values/get3?pageindex=1&pagecount=10");
        }

        [Fact]
        public void create_valid_request_using_route_and_query_string_when_parameters_are_not_primitives_with_null_properties()
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
                .Should().Be("api/values/get3?pageindex=1");
        }

        [Fact]
        public void create_valid_request_using_from_body_complex_arguments()
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
                .Should().Be("api/values/");
        }

        [Fact]
        public void create_valid_request_using_from_body_complex_arguments_with_null_properties()
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
                .Should().Be("api/values/");
        }

        [Fact]
        public void create_valid_request_using_from_form_complex_arguments()
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
                .Should().Be("api/values/post3");
        }

        [Fact]
        public void create_valid_request_using_from_form_complex_arguments_with_null_properties()
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
                .Should().Be("api/values/post3");
        }

        [Fact]
        public void create_valid_request_using_from_body_complex_arguments_and_primitive_query_parameters()
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
                .Should().Be("api/values/post/2");
        }

        [Fact]
        public void create_valid_request_using_from_body_complex_arguments_and_primitive_query_parameters_with_null_properties()
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
                .Should().Be("api/values/post/2");
        }

        [Fact]
        public void create_valid_request_using_from_form_complex_arguments_and_primitive_query_parameters()
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
                .Should().Be("api/values/post4/2");
        }

        [Fact]
        public void create_valid_request_using_from_header_primitive_arguments()
        {
            var server = new TestServerBuilder()
            .UseDefaultStartup()
            .Build();

            var header = "HeaderCustom";

            var requestPost = server.CreateHttpApiRequest<ValuesV3Controller>(
                controller => controller.Post5(header));
            requestPost.GetRequest().Headers.GetValues("custom").First().Should().Be(header);
            requestPost.GetConfiguredAddress()
               .Should().Be("api/values/post5");
        }

        [Fact]
        public void create_valid_request_using_from_header_primitive_arguments_and_from_body_complex_arguments()
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
            requestPost2.GetConfiguredAddress()
                .Should().Be("api/values/post6");
        }

        [Fact]
        public void create_valid_request_using_from_form_complex_arguments_and_primitive_query_parameters_wit_null_properties()
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
                .Should().Be("api/values/post4/2");
        }

        //[Fact]
        //public void create_valid_request_using_from_body_complex_arguments_and_complex_query_parameters()
        //{
        //    var server = new TestServerBuilder()
        //    .UseDefaultStartup()
        //    .Build();

        //    var complexParameter = new Pagination()
        //    {
        //        PageCount = 10,
        //        PageIndex = 1
        //    };

        //    var requestPost = server.CreateHttpApiRequest<ValuesV3Controller>(
        //        controller => controller.Post3(2, complexParameter, complexParameter));

        //    requestPost.GetConfiguredAddress()
        //        .Should().Be("api/values/post/2/1/10");
        //}

        [Fact]
        public void create_valid_request_when_action_use_tilde_to_override_controller_route()
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
                .Should().Be("post2/1?pageindex=1&pagecount=2");

            requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
               controller => controller.Put2(1, pagination));

            requestPost.GetConfiguredAddress()
                .Should().Be("put2/1?pageindex=1&pagecount=2");

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
                .Should().Be("post3/1?pageindex=1&pagecount=2");

            requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
               controller => controller.Put3(1, pagination));

            requestPost.GetConfiguredAddress()
                .Should().Be("put3/1?pageindex=1&pagecount=2");

            requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
                controller => controller.Delete3(1, pagination));

            requestPost.GetConfiguredAddress()
                .Should().Be("delete3/1?pageindex=1&pagecount=2");
        }

        [Fact]
        public void create_valid_request_when_action_use_tilde_to_override_controller_route_with_null_properties()
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
                .Should().Be("post2/1?pageindex=1");

            requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
               controller => controller.Put2(1, pagination));

            requestPost.GetConfiguredAddress()
                .Should().Be("put2/1?pageindex=1");

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
                .Should().Be("post3/1?pageindex=1");

            requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
               controller => controller.Put3(1, pagination));

            requestPost.GetConfiguredAddress()
                .Should().Be("put3/1?pageindex=1");

            requestPost = server.CreateHttpApiRequest<ValuesV4Controller>(
                controller => controller.Delete3(1, pagination));

            requestPost.GetConfiguredAddress()
                .Should().Be("delete3/1?pageindex=1");
        }

        [Fact]
        public async Task create_request_including_fromBody_argument_as_content_as_default_behavior()
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
        }

        [Fact]
        public async Task create_request_including_fromBody_argument_as_content_as_default_behavior_with_null_properties()
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
        }

        [Fact]
        public async Task create_request_including_fromBody_argument_as_content_configured_explicitly()
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
        }

        [Fact]
        public async Task create_request_including_fromBody_argument_as_content_configured_explicitly_with_null_properties()
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
        }

        [Fact]
        public async Task create_request_including_fromForm_argument_as_content_as_default_behavior()
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
        }

        [Fact]
        public async Task create_request_including_fromForm_argument_as_content_as_default_behavior_with_null_properties()
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
        }

        [Fact]
        public async Task create_request_including_fromForm_argument_as_content_configured_explicitly()
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
        }

        [Fact]
        public async Task create_request_including_fromForm_argument_as_content_configured_explicitly_with_null_properties()
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
        }

        [Fact]
        public async Task create_request_including_fromForm_argument_as_content_with_complex_object()
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
        public async Task create_request_including_fromForm_argument_as_content_with_complex_object_with_null_properties()
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
        public async Task create_request_not_adding_fromBody_argument_as_content()
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
        public async Task create_request_not_adding_fromBody_argument_as_content_with_null_properties()
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
        public void create_request_supporting_guid_types_on_parameters_and_numbes_on_parameters_names()
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
                .Should().Be($"api/bugs/prm1/{guidValue}");
        }

        [Fact]
        public async Task create_request_supporting_guid_array_types_on_parameters()
        {
            var server = new TestServerBuilder()
                .UseDefaultStartup()
                .Build();

            var guidList = new List<Guid> { 
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            };

            var guidArray = guidList.ToArray();

            var request = server.CreateHttpApiRequest<BugsController>(
                actionSelector: controller => controller.GuidArraySupport(guidArray),
                tokenValues: null,
                contentOptions: new NotIncludeContent());

            var responseMessage = await request.GetAsync();

            responseMessage.EnsureSuccessStatusCode();
            var response = await responseMessage.GetToAsync<Guid[]>();

            response.Should().NotBeNull();
            response.Count().Should().Be(3);
        }

        [Fact]
        public async Task create_request_supporting_guid_array_types_on_parameters_seding_method()
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
            var response = await responseMessage.GetToAsync<Guid[]>();

            response.Should().NotBeNull();
            response.Count().Should().Be(3);
        }

        [Fact]
        public void create_request_supporting_send_method_on_client_http()
        {
            var server = new TestServerBuilder()
                .UseDefaultStartup()
                .Build();

            var request = server.CreateHttpApiRequest<BugsController>(
                actionSelector: controller => controller.GuidSupport("prm1", Guid.NewGuid()),
                tokenValues: null,
                contentOptions: new NotIncludeContent());
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
}