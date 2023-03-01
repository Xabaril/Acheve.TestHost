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
            var instance = action.ArgumentValues.Any(x => x.Key == i) ? action.ArgumentValues[i].Instance : null;

            if (instance == null || type.IsPrimitiveType() || IgnoreBind(parameters[i]))
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
                string tokenValue;
                if (property.PropertyType.IsEnumerable() && property.PropertyType.GetEnumerableElementType().IsPrimitiveType())
                {
                    tokenValue = EnumerableParameterActionTokenizer.GetTokenValue((IList)value, tokenName);
                }
                else
                {
                    tokenValue = PrimitiveParameterActionTokenizer.PrimitiveValueToString(value);
                }

                tokens.AddToken(tokenName, tokenValue, isConventional: false);
            }
        }
    }

    static bool IgnoreBind(ParameterInfo parameter)
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
}
