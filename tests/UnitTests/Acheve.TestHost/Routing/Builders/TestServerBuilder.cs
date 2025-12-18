using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace UnitTests.Acheve.TestHost.Builders
{
    public class TestServerBuilder
    {
        private readonly HostBuilder _hostBuilder;

        public TestServerBuilder()
        {
            _hostBuilder = new HostBuilder();
        }

        public TestServerBuilder UseDefaultStartup()
        {
            _hostBuilder.ConfigureWebHost(webHostBuilder =>
            {
                webHostBuilder
                    .UseTestServer()
                    .UseStartup<DefaultStartup>();
            });

            return this;
        }


        public TestServer Build()
        {
            var host = _hostBuilder
                .Build();

            host.Start();

            return host.GetTestServer();
        }

        class DefaultStartup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddControllers()
                    .AddApplicationPart(Assembly.Load(new AssemblyName("UnitTests")));
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
        }
    }
}
