using Acheve.TestHost.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.AspNetCore.TestHost
{
    public static class TestServerExtensions
    {
        /// <summary>
        /// Create a <see cref="Microsoft.AspNetCore.TestHost.RequestBuilder"/> configured automatically
        /// with the generated uri from the <paramref name="actionSelector"/>.
        /// At this moment this method only resolver HTTP API using ASP.NET CORE Attribute Routing
        /// </summary>
        /// <typeparam name="TController">The controller to use</typeparam>
        /// <param name="server">The TestServer</param>
        /// <param name="actionSelector">The action selector used to discover the uri</param>
        /// <param name="tokenValues">The optional token values used to create the uri</param>
        /// <param name="contentOptions">Determines if [FromBody] arguments are included as request content. 
        ///      By default they are included as application/json content</param>
        /// <returns></returns>
        public static RequestBuilder CreateHttpApiRequest<TController>(this TestServer server,
            Expression<Func<TController, object>> actionSelector,
            object tokenValues = null,
            RequestContentOptions contentOptions = null)
            where TController : class
        {
            if (!IsController<TController>())
            {
                throw new InvalidOperationException($"The type {typeof(TController).FullName} is not a valid MVC controller.");
            }

            if (actionSelector == null)
            {
                throw new ArgumentNullException(nameof(actionSelector));
            }

            // Include content as Json by default
            contentOptions = contentOptions ?? new IncludeContentAsJson();

            var action = GetTestServerAction(actionSelector);

            if (!IsValidActionMethod(action.MethodInfo))
            {
                throw new InvalidOperationException($"The action selector is not a valid action for MVC Controller.");
            }

            //the uri discover use only attribute route conventions..

            var validUri = UriDiscover.Discover<TController>(
                action, tokenValues);

            var requestBuilder = server.CreateRequest(validUri);

            if (contentOptions.IncludeFromBodyAsContent)
            {
                AddFromBodyArgumentsToRequestBody(requestBuilder, action, contentOptions);
            }

            if (contentOptions.IncludeFromFormAsContent)
            {
                AddFromFormArgumentsToRequestForm(requestBuilder, action, contentOptions);
            }

            AddFromHeaderArgumentsToRequestForm(requestBuilder, action);

            return requestBuilder;
        }

        private static bool IsController<TController>()
        {
            const string ControllerTypeNameSuffix = "Controller";

            var typeInfo = typeof(TController);

            if (!typeInfo.IsClass)
            {
                return false;
            }

            if (typeInfo.IsAbstract)
            {
                return false;
            }

            if (!typeInfo.IsPublic)
            {
                return false;
            }

            if (typeInfo.ContainsGenericParameters)
            {
                return false;
            }

            if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
            {
                return false;
            }

            if (!typeInfo.Name.EndsWith(ControllerTypeNameSuffix, StringComparison.OrdinalIgnoreCase) &&

                !typeInfo.IsDefined(typeof(ControllerAttribute)))
            {
                return false;
            }

            return true;
        }

        private static bool IsValidActionMethod(MethodInfo methodInfo)
        {
            if (!methodInfo.IsPublic)
            {
                return false;
            }

            if (methodInfo.IsStatic)
            {
                return false;
            }

            if (methodInfo.IsAbstract && !methodInfo.IsVirtual)
            {
                return false;
            }

            if (methodInfo.IsDefined(typeof(NonActionAttribute)))
            {
                return false;
            }


            return true;
        }

        private static TestServerAction GetTestServerAction<TController>(Expression<Func<TController, object>> actionSelector)
        {
            if (actionSelector.NodeType != ExpressionType.Lambda)
            {
                throw new InvalidOperationException($"The action selector is not a valid lambda expression");
            }

            var methodCall = (MethodCallExpression)actionSelector.Body;

            var action = new TestServerAction(methodCall.Method);
            bool haveAttributeApiController = typeof(TController).GetTypeInfo().GetCustomAttribute(typeof(ApiControllerAttribute)) != null;
            bool isGetOrDelete  = action.MethodInfo.GetCustomAttributes().FirstOrDefault(attr => attr.GetType() == typeof(HttpGetAttribute)
                                                                                         || attr.GetType() == typeof(HttpDeleteAttribute)) != null;

            var index = 0;

            foreach (var item in methodCall.Arguments)
            {
                action.AddArgument(index, item, haveAttributeApiController && !isGetOrDelete);

                ++index;
            }

            return action;
        }

        private static void AddFromBodyArgumentsToRequestBody(
            RequestBuilder requestBuilder,
            TestServerAction action,
            RequestContentOptions contentOptions)
        {
            var fromBodyArgument = action.ArgumentValues.Values.SingleOrDefault(x => x.IsFromBody);

            if (fromBodyArgument != null)
            {
                requestBuilder.And(x => x.Content =
                    contentOptions.ContentBuilder(fromBodyArgument.Instance));
            }
        }

        private static void AddFromFormArgumentsToRequestForm(
            RequestBuilder requestBuilder,
            TestServerAction action,
            RequestContentOptions contentOptions)
        {
            var fromFormArgument = action.ArgumentValues.Values.SingleOrDefault(x => x.IsFromForm);

            if (fromFormArgument != null)
            {
                requestBuilder.And(x => x.Content =
                    contentOptions.ContentBuilder(fromFormArgument.Instance));
            }
        }

        private static void AddFromHeaderArgumentsToRequestForm(
           RequestBuilder requestBuilder,
           TestServerAction action)
        {
            var fromHeaderArguments = action.ArgumentValues.Values.Where(x => x.IsFromHeader);

            foreach (var fromHeaderArgument in fromHeaderArguments)
            {
                requestBuilder.And(x => x.Headers.Add(fromHeaderArgument.HeaderName, fromHeaderArgument.Instance.ToString()));
            }
        }
    }
}
