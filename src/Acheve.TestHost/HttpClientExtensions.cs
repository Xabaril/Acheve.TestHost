using Acheve.TestHost;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Adds an Authentication header to the default request headers of the <see cref="HttpClient"/>. 
        /// Uses the provided claims and "TestServer" as authentication scheme.
        /// </summary>
        /// <param name="httpClient">The httpClient instance</param>
        /// <param name="claims">The claims collection that represents the user identity</param>
        /// <returns></returns>
        public static HttpClient WithDefaultIdentity(this HttpClient httpClient, IEnumerable<Claim> claims)
        {
            var headerName =
                AuthenticationHeaderHelper.GetHeaderName(TestServerDefaults.AuthenticationScheme);

            httpClient.DefaultRequestHeaders.Add(
                name: headerName,
                value: $"{TestServerDefaults.AuthenticationScheme} {DefautClaimsEncoder.Encode(claims)}");

            return httpClient;
        }

        /// <summary>
        /// Adds an Authentication header to the default request headers of the <see cref="HttpClient"/>. 
        /// Uses the provided claims and authentication scheme.
        /// </summary>
        /// <param name="httpClient">The httpClient instance</param>
        /// <param name="claims">The claims collection that represents the user identity</param>
        /// <param name="scheme">The authentication scheme</param>
        /// <returns></returns>
        public static HttpClient WithDefaultIdentity(this HttpClient httpClient, IEnumerable<Claim> claims, string scheme)
        {
            if (string.IsNullOrWhiteSpace(scheme))
            {
                throw new ArgumentNullException(nameof(scheme));
            }

            var headerName =
                AuthenticationHeaderHelper.GetHeaderName(scheme);

            httpClient.DefaultRequestHeaders.Add(
                name: headerName,
                value: $"{scheme} {DefautClaimsEncoder.Encode(claims)}");

            return httpClient;
        }
    }
}
