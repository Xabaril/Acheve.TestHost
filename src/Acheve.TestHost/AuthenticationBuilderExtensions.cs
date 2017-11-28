using System;
using Microsoft.AspNetCore.Authentication;
using Acheve.TestHost;

namespace Acheve.AspNetCore.TestHost.Security
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddTestServerAuthentication(this AuthenticationBuilder builder)
            => builder.AddTestServerAuthentication(
                authenticationScheme: TestServerAuthenticationDefaults.AuthenticationScheme,
                configureOptions: _ => { });

        public static AuthenticationBuilder AddTestServerAuthentication(
            this AuthenticationBuilder builder,
            Action<TestServerAuthenticationOptions> configureOptions)
            => builder.AddTestServerAuthentication(
                authenticationScheme: TestServerAuthenticationDefaults.AuthenticationScheme,
                configureOptions: configureOptions);

        public static AuthenticationBuilder AddTestServerAuthentication(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            Action<TestServerAuthenticationOptions> configureOptions)
            => builder.AddTestServerAuthentication(
                authenticationScheme: authenticationScheme,
                displayName: null,
                configureOptions: configureOptions);

        public static AuthenticationBuilder AddTestServerAuthentication(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            string displayName,
            Action<TestServerAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestServerAuthenticationOptions, TestServerAuthenticationHandler>(
                authenticationScheme: authenticationScheme,
                displayName: displayName,
                configureOptions: configureOptions);
        }
    }
}