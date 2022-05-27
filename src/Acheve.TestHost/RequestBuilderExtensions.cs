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

        /// <summary>
        /// Add the given parameter and value to the request
        /// </summary>
        /// <typeparam name="T">Type of the value to be casted to string</typeparam>
        /// <param name="requestBuilder">The requestBuilder instance</param>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>RequestBuilder instance</returns>
        public static RequestBuilder AddQueryParameter<T>(this RequestBuilder requestBuilder, string name, T value)
        {
            requestBuilder.And(configure =>
            {
                var separatoChar = '?';
                if (configure.RequestUri.ToString().Contains(separatoChar))
                    separatoChar = '&';
                
                configure.RequestUri = new Uri($"{configure.RequestUri}{separatoChar}{name}={value}", UriKind.Relative);
            });

            return requestBuilder;
        }
    }
}
