using System;
using System.Collections;
using System.Collections.Generic;

namespace Acheve.TestHost.Routing.Tokenizers;

internal class EnumerableParameterActionTokenizer
    : ITokenizer
{
    public void AddTokens<TController>(TestServerAction action, TestServerTokenCollection tokens)
        where TController : class
    {
        var parameters = action.MethodInfo.GetParameters();

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameterType = parameters[i].ParameterType;
            if (!parameterType.IsEnumerable() || !parameterType.GetEnumerableElementType().IsPrimitiveType())
            {
                continue;
            }

            var arrayValues = (Array)action.ArgumentValues[i].Instance;
            if (arrayValues == null || arrayValues.Length == 0)
            {
                continue;
            }

            var tokenName = parameters[i].Name.ToLowerInvariant();
            var tokenValue = GetTokenValue(arrayValues, tokenName);
            tokens.AddToken(tokenName, tokenValue, isConventional: false);
        }
    }

    public static string GetTokenValue(IList array, string tokenName)
    {
        var list = new List<string>();

        foreach (var element in array)
        {
            list.Add(element.ToString());
        }

        return string.Join($"&{tokenName}=", list);
    }
}