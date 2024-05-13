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
        foreach (var argument in action.ArgumentValues.Values)
        {
            var parameterType = argument.Type;
            if (!parameterType.IsEnumerable() || !parameterType.GetEnumerableElementType().IsPrimitiveType())
            {
                continue;
            }

            var arrayValues = (IList)argument.Instance;
            if (arrayValues == null || arrayValues.Count == 0)
            {
                continue;
            }

            var tokenName = argument.Name.ToLowerInvariant();
            var tokenValue = GetTokenValue(arrayValues, tokenName);
            tokens.AddToken(tokenName, tokenValue, isConventional: false);
        }
    }

    public static string GetTokenValue(IList array, string tokenName)
    {
        var list = new List<string>();

        foreach (var element in array)
        {
            list.Add(PrimitiveParameterActionTokenizer.PrimitiveValueToString(element));
        }

        return string.Join($"&{tokenName}=", list);
    }
}