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
            if (!tokens.ContainsToken($"[{controller_key}]"))
            {
                var controllerName = typeof(TController).Name
                    .Replace(ControllerTypeNameSuffix, string.Empty)
                    .ToLower();

                tokens.AddToken(controller_key, controllerName, isConventional: true);
            }

            const string action_key = "action";
            if (!tokens.ContainsToken($"[{action_key}]"))
            {
                tokens.AddToken(action_key, action.MethodInfo.Name.ToLower(), isConventional: true);
            }
        }
    }
}
