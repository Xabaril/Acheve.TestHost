using Acheve.TestHost;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        public static HttpClient WithDefaultIdentity(this HttpClient httpClient, IEnumerable<Claim> claims)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    TestServerAuthenticationDefaults.AuthenticationScheme,
                    DefautClaimsEncoder.Encode(claims));

            return httpClient;
        }

       
    }
}
