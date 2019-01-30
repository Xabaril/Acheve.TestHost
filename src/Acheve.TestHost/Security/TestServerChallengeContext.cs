using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;

namespace Acheve.TestHost
{
    public class TestServerChallengeContext : PropertiesContext<TestServerOptions>
    {
        public TestServerChallengeContext(
            HttpContext context,
            AuthenticationScheme scheme,
            TestServerOptions options,
            AuthenticationProperties properties)
            : base(context, scheme, options, properties) { }

        public Exception AuthenticateFailure { get; set; }

        public string Error { get; set; }

        public bool Handled { get; private set; }

        public void HandleResponse() => Handled = true;
    }
}