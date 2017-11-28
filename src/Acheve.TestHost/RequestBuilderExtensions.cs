using Acheve.TestHost;
using System.Collections.Generic;
using System.Security.Claims;

namespace Microsoft.AspNetCore.TestHost
{
    public static class RequestBuilderExtensions
    {
        public static RequestBuilder WithIdentity(this RequestBuilder requestBuilder, IEnumerable<Claim> claims)
        {
            requestBuilder.AddHeader(
                Constants.AuthenticationHeaderName,
                $"{TestServerAuthenticationDefaults.AuthenticationScheme} {DefautClaimsEncoder.Encode(claims)}");

            return requestBuilder;
        }
    }
}
