using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Acheve.TestHost
{
    public class TokenValidatedContext : ResultContext<TestServerOptions>
    {
        public TokenValidatedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            TestServerOptions options)
            : base(context, scheme, options) { }        
    }
}