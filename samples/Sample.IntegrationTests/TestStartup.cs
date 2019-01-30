using System;
using System.Reflection;
using System.Security.Claims;
using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sample.Api;

namespace Sample.IntegrationTests
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(TestServerAuthenticationDefaults.AuthenticationScheme)
                .AddTestServerAuthentication()
                .AddTestServerAuthentication("Bearer", options =>
                 {
                     options.NameClaimType = "name";
                     options.RoleClaimType = "role";
                 });

            var mvcCoreBuilder = services.AddMvcCore()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddApplicationPart(Assembly.Load(new AssemblyName("Sample.Api")));

            ApiConfiguration.ConfigureCoreMvc(mvcCoreBuilder);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}
