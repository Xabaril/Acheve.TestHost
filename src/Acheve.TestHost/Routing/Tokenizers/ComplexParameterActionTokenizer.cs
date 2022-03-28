﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Reflection;

namespace Acheve.TestHost.Routing.Tokenizers
{
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

                if (instance != null && !type.IsPrimitiveType())
                {
                    if (!IgnoreBind(parameters[i]))
                    {
                        foreach (var property in type.GetProperties())
                        {
                            var tokenName = property.Name.ToLowerInvariant();
                            var value = property.GetValue(instance);

                            if (value != null)
                            {
                                tokens.AddToken(tokenName, value.ToString(), isConventional: false);
                            }
                        }
                    }
                }
            }
        }

        bool IgnoreBind(ParameterInfo parameter)
        {
            var attributes = parameter.GetCustomAttributes(false);

            foreach (var attribute in attributes)
            {
                switch (attribute)
                {
                    case FromBodyAttribute body: return true;
                    case FromFormAttribute form: return true;
                    case BindNeverAttribute bind: return true;
                    default: continue;
                }
            }

            return false;
        }
    }
}
