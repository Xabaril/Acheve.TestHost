[![Build status](https://ci.appveyor.com/api/projects/status/n6mfcq24ud5lecvb?svg=true)](https://ci.appveyor.com/project/Xabaril/acheve-testhost) [![NuGet](https://img.shields.io/nuget/v/acheve.testhost.svg)](https://www.nuget.org/packages/acheve.testhost/)

[![Build history](https://buildstats.info/appveyor/chart/xabaril/Acheve-TestHost)](https://ci.appveyor.com/project/xabaril/Acheve-TestHost/history)

# Acheve

NuGet package to improve  AspNetCore TestServer experiences

Unit testing your Mvc controllers is not enough to verify the correctness of your WebApi. Are the filters working? Is the correct status code sent when that condition is reached? Is the user authorized to request that endpoint? 


The NuGet package [Microsoft.AspNetCore.TestHost](https://www.nuget.org/packages/Microsoft.AspNetCore.TestHost/) allows you to create an in memory server that exposes an HttpClient to be able to send request to the server. All in memory, all in the same process. Fast. It's the best way to create integration tests in your Mvc application. But at this moment this library has some gaps that *Acheve* try to fill.

## About Security

But when your Mvc application requires an authenticated request it could be a little more dificult...

What if you have an easy way to indicate the claims in the request? 

This package implements an authentication middleware and several extension methods to easily indicate
the claims for authenticated calls to the WebApi.

In the TestServer startup class you shoud incude the authentication service and add the .Net Core new AUthentication middleware:

 ```csharp

     public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = TestServerDefaults.AuthenticationScheme;
                })
                .AddTestServer();

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
```

And in your tests you can use an HttpClient with default credentials or build 
the request with the server RequestBuilder and the desired claims:

```csharp

    public class ValuesWithDefaultUserTests : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _userHttpCient;

        public ValuesWithDefaultUserTests()
        {
            // Build the test server
            var host = new WebHostBuilder()
                .UseStartup<TestStartup>();

            _server = new TestServer(host);

            // You can create an HttpClient instance with a default identity
            _userHttpCient = _server.CreateClient()
                .WithDefaultIdentity(Identities.User);
        }

        [Fact]
        public async Task WithHttpClientWithDefautIdentity()
        {
            var response = await _userHttpCient.GetAsync("api/values");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task WithRequestBuilder()
        {
            // Or you can create a request and assign the identity to the RequestBuilder
            var response = await _server.CreateRequest("api/values")
                .WithIdentity(Identities.User)
                .GetAsync();

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Anonymous()
        {
            var response = await _server.CreateRequest("api/values/public")
                .GetAsync();

            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _server.Dispose();
            _userHttpCient.Dispose();
        }
    }
```

Both methods (`WithDefaultIdentity` and `WithIdentity`) accept as the only parameter an IEnumerabe&lt;Claim&gt; that should include the desired user claims for the request.

```csharp

    public static class Identities
    {
        public static readonly IEnumerable<Claim> User = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Name, "User"),
        };

        public static readonly IEnumerable<Claim> Empty = new Claim[0];
    }

```

You can find a complete example in the [samples](https://github.com/hbiarge/Acheve.AspNetCore.TestHost.Security/tree/master/Acheve.AspNet.TestHost.Security/samples) directory.


## About discovering uri's

Well, when you try to create any test using Test Server you need to know the uri of the action to be invoked.

```csharp

var response = await _server.CreateRequest("api/values/public")
                .GetAsync();

```

In general, in our tests a new simple API class is created to hide this uri and improve the code.

```csharp
 
 // some code on tests 

var response = await _server.CreateRequest(Api.Values.Public)
                .GetAsync();

// the API class

public static class API
{
    public static class Values
    {
        public static string Public = "api/values/public";
    }
}

```

The main problems on this approach are:

    1.- If any route convention is changed all integration test will fail.
    2.- If you refactor any parameter order the integration test will fail.

With *Acheve* you can create the uri dynamically using the attribute routing directly from your controllers.

```csharp

var response = await _server.CreateHttpApiRequest<ValuesController>(controller=>controller.PublicValues())
                .GetAsync();

```
