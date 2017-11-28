using Acheve.TestHost.Routing.Tokenizers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Acheve.TestHost.Routing.AttributeTemplates
{
    class QueryStringTemplate
        : AttributeTemplateSelector
    {
        public override IEnumerable<string> GetTemplates<TController>(TestServerAction action, TestServerTokenCollection tokens)
        {
            var builder = new StringBuilder();

            var unusedTokens = tokens.GetUnusedTokens();

            if (unusedTokens.Any())
            {
                foreach (var token in unusedTokens)
                {
                    builder.Append($"{token.Name}={token.Value}&");
                }

                builder.Length -= 1;

                return new List<string>()
                {
                    $"?{builder}"
                };
            }
            else
            {
                return new List<string>() { string.Empty };
            }
        }
    }
}
