using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection;

namespace Acheve.TestHost.Routing.Tokenizers;

internal class PrimitiveParameterActionTokenizer
    : ITokenizer
{
    public void AddTokens<TController>(TestServerAction action, TestServerTokenCollection tokens)
        where TController : class
    {
        ParameterInfo[] parameters = action.MethodInfo.GetParameters();

        for (int i = 0; i < parameters.Length; i++)
        {
            if (IgnoreHeader(parameters[i]))
            {
                continue;
            }

            Type parameterType = parameters[i].ParameterType;
            if (!parameterType.IsPrimitiveType())
            {
                continue;
            }

            object tokenValue = action.ArgumentValues.Any(x => x.Key == i) ? action.ArgumentValues[i].Instance : null;
            if (tokenValue == null)
            {
                continue;
            }

            string tokenName = parameters[i].Name.ToLowerInvariant();
            tokens.AddToken(tokenName, tokenValue.ToString(), isConventional: false);
        }
    }

    private static bool IgnoreHeader(ParameterInfo parameter)
    {
        object[] attributes = parameter.GetCustomAttributes(false);

        return attributes.Any(a => a.GetType() == typeof(FromHeaderAttribute));
    }
}