using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sample.Api;

namespace Sample.IntegrationTests
{
    public class TestStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = TestServerAuthenticationDefaults.AuthenticationScheme;
                })
                .AddTestServerAuthentication();

            var mvcCoreBuilder = services.AddMvcCore();
            ApiConfiguration.ConfigureCoreMvc(mvcCoreBuilder);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }
    }
}
