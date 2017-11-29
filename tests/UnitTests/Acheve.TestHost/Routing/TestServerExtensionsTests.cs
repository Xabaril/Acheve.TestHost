using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using System;
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
                controller => controller.Post2(2,complexParameter));

            requestPost.GetConfiguredAddress()
                .Should().Be("api/values/post/2");
        }

        [Fact]
        public void create_valid_request_using_from_body_complex_arguments_and_complex_query_parameters()
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
                controller => controller.Post3(2, complexParameter,complexParameter));

            requestPost.GetConfiguredAddress()
                .Should().Be("api/values/post/2/1/10");
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
