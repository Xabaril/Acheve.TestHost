using System;
using Acheve.TestHost;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Net.Http.Headers;

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
            return requestBuilder.WithIdentity(
                claims,
                TestServerDefaults.AuthenticationScheme);
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

        /// <summary>
        /// Get the header string for the provided claims
        /// </summary>
        /// <param name="requestBuilder">The requestBuilder instance</param>
        /// <param name="claims">The claims collection that represents the user identity</param>
        /// <returns></returns>
        public static string GetHeaderForIdentity(this RequestBuilder requestBuilder, IEnumerable<Claim> claims)
        {
            return requestBuilder.GetHeaderForIdentity(
                claims,
                TestServerDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Get the header string for the provided claims
        /// </summary>
        /// <param name="requestBuilder">The requestBuilder instance</param>
        /// <param name="claims">The claims collection that represents the user identity</param>
        /// <param name="scheme">The authentication scheme</param>
        /// <returns></returns>
        public static string GetHeaderForIdentity(this RequestBuilder requestBuilder, IEnumerable<Claim> claims, string scheme)
        {
            if (string.IsNullOrWhiteSpace(scheme))
            {
                throw new ArgumentNullException(nameof(scheme));
            }

            var headerName = AuthenticationHeaderHelper.GetHeaderName(scheme);

            var header = new NameValueHeaderValue(
                headerName,
                $"{TestServerDefaults.AuthenticationScheme} {DefautClaimsEncoder.Encode(claims)}");

            return header.ToString();
        }
    }
}
