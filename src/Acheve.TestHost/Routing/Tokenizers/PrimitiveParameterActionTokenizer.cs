using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var parameter = parameters[i];
            if (IgnoreHeader(parameter))
            {
                continue;
            }

            Type parameterType = parameter.ParameterType;
            if (!parameterType.IsPrimitiveType())
            {
                continue;
            }

            object tokenValue = action.ArgumentValues.Any(x => x.Key == i) ? action.ArgumentValues[i].Instance : null;
            if (tokenValue == null)
            {
                continue;
            }

            string tokenName = parameter.Name.ToLowerInvariant();
            tokens.AddToken(tokenName, PrimitiveValueToString(tokenValue), isConventional: false);
        }
    }

    public static string PrimitiveValueToString<T>(T value)
        => value switch
        {
            DateTime dateTimeValue => dateTimeValue.ToString("yyyy/MM/ddTHH:mm:ss.fff"),
            double doubleValue => JsonConvert.SerializeObject(doubleValue),
            _ => value.ToString()
        };

    private static bool IgnoreHeader(ParameterInfo parameter)
        => parameter
            .GetCustomAttributes(false)
            .Any(a => a.GetType() == typeof(FromHeaderAttribute));
}