using Acheve.TestHost.Routing.Tokenizers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Acheve.TestHost.Routing.AttributeTemplates
{
    class AttributeRouteTemplates : AttributeTemplateSelector
    {
        public override IEnumerable<string> GetTemplates<TController>(TestServerAction action, TestServerTokenCollection tokens)
        {
            var routeDefinitions = action.MethodInfo.GetCustomAttributes<RouteAttribute>();

            return routeDefinitions.Select(route =>
            {
                return SubstituteTokens(route.Template.ToLowerInvariant(), tokens);
            });
        }
    }
}
