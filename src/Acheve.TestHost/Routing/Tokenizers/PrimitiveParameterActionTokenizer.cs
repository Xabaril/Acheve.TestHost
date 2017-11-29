using System;

namespace Acheve.TestHost.Routing.Tokenizers
{
    class PrimitiveParameterActionTokenizer
        : ITokenizer
    {
        public void AddTokens<TController>(TestServerAction action, TestServerTokenCollection tokens)
            where TController : class
        {
            var parameters = action.MethodInfo.GetParameters();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.IsPrimitive 
                    ||
                    parameters[i].ParameterType == typeof(String)
                    ||
                    parameters[i].ParameterType == typeof(Decimal))
                {
                    var tokenName = parameters[i].Name.ToLowerInvariant();
                    var tokenValue = action.ArgumentValues[i];

                    tokens.AddToken(tokenName, tokenValue.ToString(), isConventional: false);
                }
            }
        }
    }
}
