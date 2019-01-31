using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Acheve.TestHost
{
    public class TestServerHandler : AuthenticationHandler<TestServerOptions>
    {
        public TestServerHandler(
            IOptionsMonitor<TestServerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected new TestServerEvents Events
        {
            get => (TestServerEvents)base.Events;
            set => base.Events = value;
        }

        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new TestServerEvents());

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = null;
            try
            {
                // Give application opportunity to find from a different location, adjust, or reject token
                var messageReceivedContext = new MessageReceivedContext(Context, Scheme, Options);

                // event can set the token
                await Events.MessageReceived(messageReceivedContext);
                if (messageReceivedContext.Result != null)
                {
                    return messageReceivedContext.Result;
                }

                // If application retrieved token from somewhere else, use that.
                token = messageReceivedContext.Token;

                // If not, use the default location
                if (string.IsNullOrEmpty(token))
                {
                    var headerName = AuthenticationHeaderHelper.GetHeaderName(Scheme.Name);
                    string authorization = Context.Request.Headers[headerName];

                    // If no authorization header found, nothing to process further
                    if (string.IsNullOrEmpty(authorization))
                    {
                        return AuthenticateResult.NoResult();
                    }

                    if (authorization.StartsWith($"{Scheme.Name} ", StringComparison.OrdinalIgnoreCase))
                    {
                        token = authorization.Substring($"{Scheme.Name} ".Length).Trim();
                    }

                    // If no token found, no further work possible
                    if (string.IsNullOrEmpty(token))
                    {
                        return AuthenticateResult.NoResult();
                    }
                }

                var claims = DefautClaimsEncoder.Decode(token).ToArray();

                var principal = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        claims: Options.CommonClaims.Union(claims),
                        authenticationType: Scheme.Name,
                        nameType: Options.NameClaimType,
                        roleType: Options.RoleClaimType));

                Logger.LogInformation("{Scheme} Authenticated", Scheme.Name);

                var tokenValidatedContext = new TokenValidatedContext(Context, Scheme, Options)
                {
                    Principal = principal
                };

                await Events.TokenValidated(tokenValidatedContext);
                if (tokenValidatedContext.Result != null)
                {
                    return tokenValidatedContext.Result;
                }

                tokenValidatedContext.Success();
                return tokenValidatedContext.Result;
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Authentication failed for schema: {Scheme}. {message}", Scheme.Name, ex.Message);
                var authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options)
                {
                    Exception = ex
                };

                await Events.AuthenticationFailed(authenticationFailedContext);
                if (authenticationFailedContext.Result != null)
                {
                    return authenticationFailedContext.Result;
                }

                throw;
            }
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authResult = await HandleAuthenticateOnceSafeAsync();
            var eventContext = new TestServerChallengeContext(Context, Scheme, Options, properties)
            {
                AuthenticateFailure = authResult?.Failure
            };

            // Avoid returning error=invalid_token if the error is not caused by an authentication failure (e.g missing token).
            if (eventContext.AuthenticateFailure != null)
            {
                eventContext.Error = "invalid_token";
            }

            await Events.Challenge(eventContext);
            if (eventContext.Handled)
            {
                return;
            }

            Response.StatusCode = 401;

            if (string.IsNullOrEmpty(eventContext.Error))
            {
                Response.Headers.Append(HeaderNames.WWWAuthenticate, Scheme.Name);
            }
            else
            {
                // https://tools.ietf.org/html/rfc6750#section-3.1
                // WWW-Authenticate: Bearer realm="example", error="invalid_token", error_description="The access token expired"
                Response.Headers.Append(HeaderNames.WWWAuthenticate, $"{Scheme.Name} realm=\"test\", error=\"{eventContext.Error}\"");
            }
        }
    }
}
