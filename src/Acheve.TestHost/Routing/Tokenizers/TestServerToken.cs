using System;

namespace Acheve.TestHost.Routing.Tokenizers
{
    public class TestServerToken
    {
        public string Name { get; private set; }

        public string Value { get; private set; }

        public bool IsConventional { get; private set; }

        public bool Used { get; private set; }

        public TestServerToken(string name, string value, bool isConventional)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
            IsConventional = isConventional;
        }

        public void SetAsUsed()
        {
            Used = true;
        }
    }
}
