using System.Collections.Generic;
using System.Linq;

namespace Acheve.TestHost.Routing.Tokenizers;

public class TestServerTokenCollection
{
    private readonly Dictionary<string, TestServerToken> _activeTokens = new();

    public bool ContainsToken(string tokenName)
    {
        return _activeTokens.ContainsKey(tokenName);
    }

    public void AddToken(string tokenName, string tokenValue, bool isConventional = false)
    {
        if (!ContainsToken(tokenName))
        {
            _activeTokens.Add(tokenName,
                new TestServerToken(tokenName, tokenValue, isConventional));
        }
    }

    public TestServerToken Find(string tokenName)
        => ContainsToken(tokenName) ? _activeTokens[tokenName] : default;

    public IEnumerable<TestServerToken> GetConventionalTokens()
    {
        return _activeTokens.Values
            .Where(token => token.IsConventional);
    }

    public IEnumerable<TestServerToken> GetNonConventionalTokens()
    {
        return _activeTokens.Values
            .Where(token => !token.IsConventional);
    }

    public IEnumerable<TestServerToken> GetUnusedTokens()
    {
        return _activeTokens.Values
            .Where(token => !token.IsConventional && !token.Used);
    }

    public static TestServerTokenCollection FromDictionary(Dictionary<string, string> tokenKV)
    {
        var tokens = new TestServerTokenCollection();

        foreach (var item in tokenKV)
        {
            tokens.AddToken(item.Key, item.Value, isConventional: false);
        }

        return tokens;
    }
}
