using System.Net.Http;

namespace Microsoft.AspNetCore.TestHost
{
    public static class RequestBuilderIntrospection
    {
        public static string GetConfiguredAddress(this RequestBuilder requestBuilder)
        {
            var req = typeof(RequestBuilder).GetField("_req", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);


            var message = (HttpRequestMessage)req.GetValue(requestBuilder);

            return message.RequestUri.ToString();
        }
    }
}
