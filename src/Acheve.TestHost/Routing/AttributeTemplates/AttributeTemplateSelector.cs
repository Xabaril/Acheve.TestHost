using Acheve.TestHost.Routing.Tokenizers;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Acheve.TestHost.Routing.AttributeTemplates
{
    abstract class AttributeTemplateSelector
    {
        public abstract IEnumerable<string> GetTemplates<TController>(TestServerAction action,
            TestServerTokenCollection tokens) where TController : class;

        public virtual string SubstituteTokens(string template, TestServerTokenCollection tokens)
        {
            var regex_pattern = "{[a-zA-Z0-9?]*:??[a-zA-Z0-9]*}";

            template = template.ToLowerInvariant();

            foreach (var token in tokens.GetConventionalTokens())
            {
                var conventional_token = $"[{token.Name}]";

                if (template.Contains(conventional_token))
                {
                    template = template.Replace(conventional_token, token.Value);

                    token.SetAsUsed();
                }
            }

            var matches = Regex.Matches(template, regex_pattern, RegexOptions.Compiled);

            foreach (Match match in matches)
            {
                string parameter = null;

                regex_pattern = @"{([^\?:}]*)";
                parameter = Regex.Match(match.Value, regex_pattern)
                    .Groups
                    .Values
                    .LastOrDefault()?
                    .Value;

                var token = tokens.Find(parameter);

                if (token != default(TestServerToken))
                {
                    template = template.Replace(match.Value, token.Value);

                    token.SetAsUsed();
                }
            }

            regex_pattern = @"\/{[^{}]*\?}";
            template = Regex.Replace(template, regex_pattern, string.Empty);

            return template.ToLowerInvariant();
        }
    }
}
