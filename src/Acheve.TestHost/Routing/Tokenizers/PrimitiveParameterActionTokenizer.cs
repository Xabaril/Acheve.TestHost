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
                if (!IgnoreHeader(parameters[i])) {

                    if (IsPrimitiveType(parameters[i].ParameterType))
                    {
                        var tokenName = parameters[i].Name.ToLowerInvariant();
                        var tokenValue = action.ArgumentValues[i].Instance;

                        if (tokenValue != null)
                        {
                            tokens.AddToken(tokenName, tokenValue.ToString(), isConventional: false);
                        }
                    }
                    else if (parameters[i].ParameterType.IsArray
                       && IsPrimitiveType(parameters[i].ParameterType.GetElementType()))
                    {
                        dynamic tokenValue = action.ArgumentValues[i].Instance;

                        if (tokenValue != null
                            && tokenValue.Length != 0
                            )
                        {
                            var tokenName = parameters[i].Name.ToLowerInvariant();
                            var value = string.Join($"&{tokenName}=", tokenValue);
                            tokens.AddToken(tokenName, value, isConventional: false);
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
    }
}