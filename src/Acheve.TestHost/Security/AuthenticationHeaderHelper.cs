namespace Acheve.TestHost
{
    internal static class AuthenticationHeaderHelper
    {
        public static string GetHeaderName(string scheme)
        {
            return $"X-TestServerAuthentication-{scheme}";
        }
    }
}
