using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sample.Api;
using System.Reflection;
using System.Threading.Tasks;

namespace Sample.IntegrationTests
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(TestServerDefaults.AuthenticationScheme)
                .AddTestServer(options =>
                {
                    options.Events = new TestServerEvents
                    {
                        OnMessageReceived = context => Task.CompletedTask,
                        OnTokenValidated = context => Task.CompletedTask,
                        OnAuthenticationFailed = context => Task.CompletedTask,
                        OnChallenge = context => Task.CompletedTask
                    };
                })
                .AddTestServer("Bearer", options =>
                 {
                     options.NameClaimType = "name";
                     options.RoleClaimType = "role";
                     options.Events = new TestServerEvents
                     {
                         OnMessageReceived = context => Task.CompletedTask,
                         OnTokenValidated = context => Task.CompletedTask,
                         OnAuthenticationFailed = context => Task.CompletedTask,
                         OnChallenge = context => Task.CompletedTask
                     };
                 });

            services.AddControllers()
                .AddApplicationPart(Assembly.Load(new AssemblyName("Sample.Api")));

            ApiConfiguration.Configure(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
