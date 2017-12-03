using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Acheve.TestHost
{
    public class TestServerAuthenticationHandler : AuthenticationHandler<TestServerAuthenticationOptions>
    {
        public TestServerAuthenticationHandler(
            IOptionsMonitor<TestServerAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headerName = AuthenticationHeaderHelper.GetHeaderName(Scheme.Name);

            StringValues authHeaderString;
            var existAuthorizationHeader =
                Context.Request.Headers.TryGetValue(headerName, out authHeaderString);

            if (existAuthorizationHeader == false)
            {
                Logger.LogInformation("{Scheme} No {HeaderName} header present", Scheme.Name, headerName);
                return Task.FromResult(AuthenticateResult.Fail("No Authorization header present"));
            }

            AuthenticationHeaderValue authHeader;
            var canParse = AuthenticationHeaderValue.TryParse(authHeaderString[0], out authHeader);

            if (canParse == false)
            {
                Logger.LogInformation("{Scheme} {HeaderName} header not valid", Scheme.Name, headerName);
                return Task.FromResult(AuthenticateResult.Fail("Authorization header not valid"));
            }

            var headerClaims = DefautClaimsEncoder.Decode(authHeader.Parameter).ToArray();

            if (headerClaims.Length == 0)
            {
                Logger.LogInformation("{Scheme} Invalid claims", Scheme.Name);
                return Task.FromResult(AuthenticateResult.Fail("Invalid claims"));
            }

            var identity = new ClaimsIdentity(
                claims: Options.CommonClaims.Union(headerClaims),
                authenticationType: Scheme.Name,
                nameType: Options.NameClaimType,
                roleType: Options.RoleClaimType);

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                Scheme.Name);
            
            Logger.LogInformation("{Scheme} Authenticated", Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
