using System.Text.Json;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Try to extract the error message in the response content in case the response status code is not success.
        /// </summary>
        /// <param name="response">The httpResponseMessage instance</param>
        /// <returns></returns>
        public static async Task IsSuccessStatusCodeOrThrow(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var content = await response.Content.ReadAsStringAsync();

            throw new Exception($"Response status does not indicate success: {response.StatusCode:D} ({response.StatusCode}); \r\n{content}");
        }

        public static async Task<T> GetToAsync<T>(this HttpResponseMessage responseMessage)
        {
            var json = await responseMessage.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}