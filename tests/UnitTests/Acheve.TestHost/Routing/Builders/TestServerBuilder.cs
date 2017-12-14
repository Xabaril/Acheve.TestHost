using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.Acheve.TestHost.Builders
{
    public class TestServerBuilder
    {
        private readonly WebHostBuilder _webHostBuilder;

        public TestServerBuilder()
        {
            _webHostBuilder = new WebHostBuilder();
            
        }

        public TestServerBuilder UseDefaultStartup()
        {
            _webHostBuilder.UseStartup<DefaultStartup>();

            return this;
        }


        public TestServer Build()
        {
            return new TestServer(_webHostBuilder);
        }

        class DefaultStartup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddMvc();
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IHostingEnvironment env)
            {
                app.UseMvcWithDefaultRoute();
            }
        }
    }
}
