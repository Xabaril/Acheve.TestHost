using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Acheve.TestHost.Routing.Tokenizers;

class ComplexParameterActionTokenizer
    : ITokenizer
{
    public void AddTokens<TController>(TestServerAction action, TestServerTokenCollection tokens) where TController : class
    {
        var parameters = action.MethodInfo.GetParameters();

        for (int i = 0; i < parameters.Length; i++)
        {
            var type = parameters[i].ParameterType;
            var argument = action.ArgumentValues.Any(x => x.Key == i) ? action.ArgumentValues[i] : null;
            var instance = argument?.Instance;

            if (instance is null || type.IsPrimitiveType() || IgnoreBind(parameters[i]) || !IsQueryOrRouteParameter(argument))
            {
                continue;
            }

            foreach (var property in type.GetProperties())
            {
                var value = property.GetValue(instance);
                if (value == null)
                {
                    continue;
                }

                var tokenName = property.Name.ToLowerInvariant();
                string tokenValue =
                    property.PropertyType.IsEnumerable() && property.PropertyType.GetEnumerableElementType().IsPrimitiveType() ?
                    EnumerableParameterActionTokenizer.GetTokenValue((IList)value, tokenName) :
                    PrimitiveParameterActionTokenizer.PrimitiveValueToString(value);

                tokens.AddToken(tokenName, tokenValue, isConventional: false);
            }
        }
    }

    private static bool IgnoreBind(ParameterInfo parameter)
    {
        var attributes = parameter.GetCustomAttributes(false);

        foreach (var attribute in attributes)
        {
            switch (attribute)
            {
                case FromBodyAttribute:
                case FromFormAttribute:
                case BindNeverAttribute:
                    return true;
                default: continue;
            }
        }

        return false;
    }

    private static bool IsQueryOrRouteParameter(TestServerArgument argument)
        => !(argument.IsFromHeader || argument.IsFromForm || argument.IsFromBody);
}
