using System;
using Microsoft.AspNetCore.Authentication;
using Acheve.TestHost;

namespace Acheve.AspNetCore.TestHost.Security
{
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddTestServer(this AuthenticationBuilder builder)
            => builder.AddTestServer(
                authenticationScheme: TestServerDefaults.AuthenticationScheme,
                displayName: null,
                configureOptions: _ => { });

        public static AuthenticationBuilder AddTestServer(
            this AuthenticationBuilder builder,
            string authenticationScheme)
            => builder.AddTestServer(
                authenticationScheme: authenticationScheme,
                displayName: null,
                configureOptions: _ => { });

        public static AuthenticationBuilder AddTestServer(
            this AuthenticationBuilder builder,
            Action<TestServerOptions> configureOptions)
            => builder.AddTestServer(
                authenticationScheme: TestServerDefaults.AuthenticationScheme,
                displayName: null,
                configureOptions: configureOptions);

        public static AuthenticationBuilder AddTestServer(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            Action<TestServerOptions> configureOptions)
            => builder.AddTestServer(
                authenticationScheme: authenticationScheme,
                displayName: null,
                configureOptions: configureOptions);

        public static AuthenticationBuilder AddTestServer(
            this AuthenticationBuilder builder,
            string authenticationScheme,
            string displayName,
            Action<TestServerOptions> configureOptions)
        {
            return builder.AddScheme<TestServerOptions, TestServerHandler>(
                authenticationScheme: authenticationScheme,
                displayName: displayName,
                configureOptions: configureOptions);
        }
    }
}