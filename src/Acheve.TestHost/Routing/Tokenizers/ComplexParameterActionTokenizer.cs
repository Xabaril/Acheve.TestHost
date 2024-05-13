using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections;
using System.Reflection;

namespace Acheve.TestHost.Routing.Tokenizers;

class ComplexParameterActionTokenizer
    : ITokenizer
{
    public void AddTokens<TController>(TestServerAction action, TestServerTokenCollection tokens) where TController : class
    {
        foreach (var argument in action.ArgumentValues.Values)
        {
            var type = argument.Type;
            var instance = argument.Instance;

            if (instance is null || type.IsPrimitiveType() || argument.NeverBind || !IsQueryOrRouteParameter(argument))
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
        => argument.FromType == TestServerArgumentFromType.None || argument.FromType.HasFlag(TestServerArgumentFromType.Query) || argument.FromType.HasFlag(TestServerArgumentFromType.Route);
}
