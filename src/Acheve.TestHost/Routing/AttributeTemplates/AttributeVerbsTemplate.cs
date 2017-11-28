using Acheve.TestHost.Routing.Tokenizers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Acheve.TestHost.Routing.AttributeTemplates
{
    class AttributeVerbsTemplate : AttributeTemplateSelector
    {
        public override IEnumerable<string> GetTemplates<TController>(TestServerAction action, TestServerTokenCollection tokens)
        {
            var getAttributes = action.MethodInfo.GetCustomAttributes<HttpPostAttribute>()
                .OfType<HttpMethodAttribute>();

            var postAttributes = action.MethodInfo.GetCustomAttributes<HttpGetAttribute>()
                .OfType<HttpMethodAttribute>();

            var putAttributes = action.MethodInfo.GetCustomAttributes<HttpPutAttribute>()
                .OfType<HttpMethodAttribute>();

            var deleteAttributes = action.MethodInfo.GetCustomAttributes<HttpDeleteAttribute>()
                .OfType<HttpMethodAttribute>();

            var verbAttribute = getAttributes
                .Union(postAttributes)
                .Union(putAttributes)
                .Union(deleteAttributes)
                .SingleOrDefault();

            if (verbAttribute?.Template != null)
            {
                return new List<string>()
                {
                    SubstituteTokens(verbAttribute.Template.ToLowerInvariant(), tokens)
                };
            }

            return Enumerable.Empty<string>();
        }
    }
}
