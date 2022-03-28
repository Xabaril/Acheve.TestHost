using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Acheve.TestHost.Routing.Tokenizers
{
    internal class PrimitiveParameterActionTokenizer
        : ITokenizer
    {
        public void AddTokens<TController>(TestServerAction action, TestServerTokenCollection tokens)
            where TController : class
        {
            var parameters = action.MethodInfo.GetParameters();

            for (var i = 0; i < parameters.Length; i++)
            {
                if (!IgnoreHeader(parameters[i]))
                {
                    var tokenName = parameters[i].Name.ToLowerInvariant();

                    if (parameters[i].ParameterType.IsPrimitiveType())
                    {
                        var tokenValue = action.ArgumentValues.Any(x => x.Key == i) ? action.ArgumentValues[i].Instance : null;

                        if (tokenValue != null)
                        {
                            tokens.AddToken(tokenName, tokenValue.ToString(), isConventional: false);
                        }
                    }
                    else if (parameters[i].ParameterType.IsArray
                       && IsPrimitiveType(parameters[i].ParameterType.GetElementType()))
                    {
                        var arrayValues = (Array)action.ArgumentValues[i].Instance;

                        if (arrayValues != null
                            && arrayValues.Length != 0
                            )
                        {
                            var tokenValue = GetTokenValue(arrayValues, tokenName);
                            tokens.AddToken(tokenName, tokenValue, isConventional: false);
                        }
                    }
                }
            }
        }

        private bool IgnoreHeader(ParameterInfo parameter)
        {
            var attributes = parameter.GetCustomAttributes(false);

            if (attributes.Any(a => a.GetType() == typeof(FromHeaderAttribute)))
            {
                return true;
            }

            return false;
        }

        private bool IsPrimitiveType(Type type)
        {
            return type.IsPrimitive
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(Guid);
        }

        private string GetTokenValue(Array array, string tokenName)
        {
            var list = new List<string>();

            foreach (var element in array)
            {
                list.Add(element.ToString());
            }

            return string.Join($"&{tokenName}=", list);
        }
    }
}