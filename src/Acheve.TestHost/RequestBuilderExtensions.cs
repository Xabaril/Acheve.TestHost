using System;
using Acheve.TestHost;
using System.Collections.Generic;
using System.Security.Claims;

namespace Microsoft.AspNetCore.TestHost
{
    public static class RequestBuilderExtensions
    {
        /// <summary>
        /// Adds an Authentication header to the <see cref="RequestBuilder"/> with the provided claims using
        /// "TestServer" as authentication scheme.
        /// </summary>
        /// <param name="requestBuilder">The requestBuilder instance</param>
        /// <param name="claims">The claims collection that represents the user identity</param>
        /// <returns></returns>
        public static RequestBuilder WithIdentity(this RequestBuilder requestBuilder, IEnumerable<Claim> claims)
        {
            var headerName =
                AuthenticationHeaderHelper.GetHeaderName(TestServerDefaults.AuthenticationScheme);

            requestBuilder.AddHeader(
                headerName,
                $"{TestServerDefaults.AuthenticationScheme} {DefautClaimsEncoder.Encode(claims)}");

            return requestBuilder;
        }

        /// <summary>
        /// Adds an Authentication header to the <see cref="RequestBuilder"/> with the provided claims 
        /// and authentication scheme.
        /// </summary>
        /// <param name="requestBuilder">The requestBuilder instance</param>
        /// <param name="claims">The claims collection that represents the user identity</param>
        /// <param name="scheme">The authentication scheme</param>
        /// <returns></returns>
        public static RequestBuilder WithIdentity(this RequestBuilder requestBuilder, IEnumerable<Claim> claims, string scheme)
        {
            if (string.IsNullOrWhiteSpace(scheme))
            {
                throw new ArgumentNullException(nameof(scheme));
            }

            var headerName = AuthenticationHeaderHelper.GetHeaderName(scheme);

            requestBuilder.AddHeader(
                headerName,
                $"{scheme} {DefautClaimsEncoder.Encode(claims)}");

            return requestBuilder;
        }
    }
}
