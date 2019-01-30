using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Acheve.TestHost
{
    public class MessageReceivedContext : ResultContext<TestServerOptions>
    {
        public MessageReceivedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            TestServerOptions options)
            : base(context, scheme, options) { }

        public string Token { get; set; }
    }
}