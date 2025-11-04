using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Sample.Api
{
    public static class ApiConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ValidateClaims", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireAssertion(context =>
                    {
                        var principal = context.User;
                        var nameIdentifierClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

                        if (nameIdentifierClaim == null
                            || nameIdentifierClaim.Value != "1"
                            || nameIdentifierClaim.ValueType != ClaimValueTypes.Integer32
                            || nameIdentifierClaim.Issuer != "TestIssuer"
                            || nameIdentifierClaim.OriginalIssuer != "OriginalTestIssuer")
                        {
                            return false;
                        }

                        return true;
                    });
                });
            });
        }
    }
}
