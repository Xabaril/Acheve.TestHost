using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Sample.Api
{
    public static class ApiConfiguration
    {
        public static void ConfigureCoreMvc(IMvcCoreBuilder builder)
        {
            builder.AddAuthorization();
            builder.AddJsonFormatters(options =>
            {
                options.NullValueHandling = NullValueHandling.Ignore;
            });
        }

        public static void ConfigureMvc(IMvcBuilder builder)
        {
            // Configure full registered Mvc services
        }

        public static void Configure(IServiceCollection services)
        {
            // Configure other services

        }
    }
}
