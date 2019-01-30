using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;

namespace Acheve.TestHost
{
    public class AuthenticationFailedContext : ResultContext<TestServerOptions>
    {
        public AuthenticationFailedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            TestServerOptions options)
            : base(context, scheme, options) { }

        public Exception Exception { get; set; }
    }
}