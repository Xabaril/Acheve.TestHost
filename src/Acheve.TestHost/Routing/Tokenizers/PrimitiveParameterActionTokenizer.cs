﻿using Microsoft.AspNetCore.Mvc;
using System;
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

                    if (tokenValue != null)
                    {
                        tokens.AddToken(tokenName, tokenValue.ToString(), isConventional: false);
                    }
                }
                else if (parameters[i].ParameterType.IsArray
                   && !IgnoreHeader(parameters[i]))
                {
                    dynamic tokenValue = action.ArgumentValues[i].Instance;

                    if (tokenValue != null 
                        && tokenValue.Length != 0 
                        && (tokenValue[0].GetType().IsPrimitive
                            ||
                            tokenValue[0].GetType() == typeof(string)
                            ||
                            tokenValue[0].GetType() == typeof(decimal)
                            ||
                            tokenValue[0].GetType() == typeof(Guid))
                        )
                    {
                        var tokenName = parameters[i].Name.ToLowerInvariant();
                        var value = string.Join($"&{tokenName}=", tokenValue);
                        tokens.AddToken(tokenName, value, isConventional: false);
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
    }
}