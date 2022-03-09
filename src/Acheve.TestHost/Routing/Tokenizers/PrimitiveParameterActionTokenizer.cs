using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection;

namespace Acheve.TestHost.Routing.Tokenizers
{
    class PrimitiveParameterActionTokenizer
        : ITokenizer
    {
        public void AddTokens<TController>(TestServerAction action, TestServerTokenCollection tokens)
            where TController : class
        {
            var parameters = action.MethodInfo.GetParameters();

            for (var i = 0; i < parameters.Length; i++)
            {
                if ((parameters[i].ParameterType.IsPrimitive
                    ||
                    parameters[i].ParameterType == typeof(string)
                    ||
                    parameters[i].ParameterType == typeof(decimal)
                    ||
                    parameters[i].ParameterType == typeof(Guid))
                    && !IgnoreHeader(parameters[i]))
                {
                    var tokenName = parameters[i].Name.ToLowerInvariant();
                    var tokenValue = action.ArgumentValues[i].Instance;

                    tokens.AddToken(tokenName, tokenValue.ToString(), isConventional: false);
                }
            }
        }

        bool IgnoreHeader(ParameterInfo parameter)
        {
            var attributes = parameter.GetCustomAttributes(false);

            if (attributes.Any(a => a.GetType() == typeof(FromHeaderAttribute)))
            {
                return true;
            }

            return false;
        }
    }
}
