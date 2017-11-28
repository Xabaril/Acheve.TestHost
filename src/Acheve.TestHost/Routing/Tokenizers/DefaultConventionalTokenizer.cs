using System;

namespace Acheve.TestHost.Routing.Tokenizers
{
    class DefaultConventionalTokenizer
        : ITokenizer
    {
        public void AddTokens<TController>(TestServerAction action, TestServerTokenCollection tokens)
            where TController : class
        {
            const string ControllerTypeNameSuffix = "Controller";

            const string controller_key = "controller";

            if (!tokens.ContainsToken(controller_key))
            {
                var controllerName = typeof(TController).Name
                    .Replace(ControllerTypeNameSuffix, String.Empty);

                tokens.AddToken(controller_key, controllerName, isConventional: true);
            }
        }
    }
}
