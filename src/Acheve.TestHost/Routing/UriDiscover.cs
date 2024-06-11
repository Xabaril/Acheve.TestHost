using Acheve.TestHost.Routing.AttributeTemplates;
using Acheve.TestHost.Routing.Tokenizers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Acheve.TestHost.Routing;

internal static class UriDiscover
{
    private static readonly IEnumerable<ITokenizer> _tokenizers = new List<ITokenizer>()
    {
        new PrimitiveParameterActionTokenizer(),
        new EnumerableParameterActionTokenizer(),
        new ComplexParameterActionTokenizer(),
        new DefaultConventionalTokenizer()
    };

    public static RequestBuilder CreateHttpApiRequest<TController>(TestServer server,
           LambdaExpression actionSelector,
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

        var action = GetTestServerAction<TController>(actionSelector);

        if (!IsValidActionMethod(action.MethodInfo))
        {
            throw new InvalidOperationException($"The action selector is not a valid action for MVC Controller.");
        }

        //the uri discover use only attribute route conventions.

        var validUri = Discover<TController>(action, tokenValues);

        var requestBuilder = server.CreateRequest(validUri);

        // Include content as Json by default
        contentOptions ??= action.ArgumentValues.Values.Any(a => a.FromType.HasFlag(TestServerArgumentFromType.Form))
            ? new IncludeContentAsFormUrlEncoded()
            : new IncludeContentAsJson();

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

    private static string Discover<TController>(TestServerAction action, object tokenValues)
        where TController : class
    {
        //at this moment only the first route is considered.. 

        var testServerTokens = AddTokens<TController>(action, tokenValues);

        var controllerTemplate = new AttributeControllerRouteTemplates()
            .GetTemplates<TController>(action, testServerTokens)
            .FirstOrDefault();

        var verbsTemplate = new AttributeVerbsTemplate()
            .GetTemplates<TController>(action, testServerTokens)
            .FirstOrDefault();

        var routeTemplate = new AttributeRouteTemplates()
          .GetTemplates<TController>(action, testServerTokens)
          .FirstOrDefault();

        var queryStringTemplate = new QueryStringTemplate()
            .GetTemplates<TController>(action, testServerTokens)
            .FirstOrDefault();

        var template = (verbsTemplate ?? routeTemplate);

        if (template is null)
        {
            return $"{controllerTemplate}{queryStringTemplate}";
        }

        return IsTildeOverride(template, out string overrideTemplate) ?
            $"{overrideTemplate}{queryStringTemplate}" :
            $"{controllerTemplate}/{template}{queryStringTemplate}";
    }

    private static TestServerTokenCollection AddTokens<TController>(TestServerAction action, object tokenValues)
        where TController : class
    {
        var dictionaryTokenValues = new Dictionary<string, string>();

        if (tokenValues != null)
        {
            dictionaryTokenValues = tokenValues.GetType()
                .GetProperties()
                .ToDictionary(p => p.Name.ToLowerInvariant(), p => p.GetValue(tokenValues).ToString());
        }

        var testServerTokens = TestServerTokenCollection.FromDictionary(dictionaryTokenValues);

        foreach (var tokeniker in _tokenizers)
        {
            tokeniker.AddTokens<TController>(action, testServerTokens);
        }

        return testServerTokens;
    }

    private static bool IsTildeOverride(string template, out string overrideTemplate)
    {
        const string TILDE = "~";

        overrideTemplate = null;
        var isTildeOverride = template.StartsWith(TILDE);

        if (isTildeOverride)
        {
            overrideTemplate = template[2..]; // remove ~/
        }

        return isTildeOverride;
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

    private static TestServerAction GetTestServerAction<TController>(LambdaExpression actionSelector)
    {
        if (actionSelector.NodeType != ExpressionType.Lambda)
        {
            throw new InvalidOperationException($"The action selector is not a valid lambda expression");
        }

        var methodCall = (MethodCallExpression)actionSelector.Body;

        var action = new TestServerAction(methodCall.Method);
        bool haveAttributeApiController = typeof(TController).GetTypeInfo().GetCustomAttribute(typeof(ApiControllerAttribute)) != null;
        bool isGetOrDelete = action.MethodInfo.GetCustomAttributes().FirstOrDefault(attr => attr.GetType() == typeof(HttpGetAttribute)
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
        var fromBodyArgument = action.ArgumentValues.Values.SingleOrDefault(x => x.FromType.HasFlag(TestServerArgumentFromType.Body));

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
        var fromFormArgument = action.ArgumentValues.Values.SingleOrDefault(x => x.FromType.HasFlag(TestServerArgumentFromType.Form));

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
        var fromHeaderArguments = action.ArgumentValues.Values.Where(x => x.FromType.HasFlag(TestServerArgumentFromType.Header));

        foreach (var fromHeaderArgument in fromHeaderArguments)
        {
            requestBuilder.And(x => x.Headers.Add(fromHeaderArgument.Name, fromHeaderArgument.Instance.ToString()));
        }
    }
}
