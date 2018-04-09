using Acheve.TestHost.Routing.Tokenizers;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Acheve.TestHost.Routing.AttributeTemplates
{
    abstract class AttributeTemplateSelector
    {
        public abstract IEnumerable<string> GetTemplates<TController>(TestServerAction action,
            TestServerTokenCollection tokens) where TController : class;

        public virtual string SubstituteTokens(string template, TestServerTokenCollection tokens)
        {
            const string regex_pattern = "{[a-zA-Z0-9]*:??[a-zA-Z0-9]*}";

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

                if (match.Value.Contains(":"))
                {
                    //has constraints
                    parameter = match.Value.Split(':')[0]
                        .Remove(0, 1); // remove first bracket
                }
                else
                {
                    parameter = match.Value.Replace("{", string.Empty)
                        .Replace("}", string.Empty);
                }

                var token = tokens.Find(parameter);

                if (token != default(TestServerToken))
                {
                    template = template.Replace(match.Value, token.Value);

                    token.SetAsUsed();
                }
            }

            return template;
        }
    }
}
