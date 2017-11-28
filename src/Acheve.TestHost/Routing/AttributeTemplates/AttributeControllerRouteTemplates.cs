using Acheve.TestHost.Routing.Tokenizers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Acheve.TestHost.Routing.AttributeTemplates
{
    class AttributeControllerRouteTemplates : AttributeTemplateSelector
    {
        public override IEnumerable<string> GetTemplates<TController>(TestServerAction action,TestServerTokenCollection tokens)
        {
            if (!typeof(TController).IsDefined(typeof(RouteAttribute)))
            {
                return Enumerable.Empty<string>();
            }

            var routeDefinitions = typeof(TController)
                .GetCustomAttributes<RouteAttribute>();

            return routeDefinitions.Select(route =>
            {
                return SubstituteTokens(route.Template.ToLowerInvariant(),tokens);
            });
        }
    }
}
