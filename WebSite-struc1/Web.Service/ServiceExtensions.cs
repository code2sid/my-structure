using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace Web.Service
{
    public static class ServiceExtensions
    {
        public static async Task<HttpResponseMessage> CheckStatus(this HttpResponseMessage response)
        {
            return await response.CheckStatus<HttpError>(error => error.Message);
        }

        public static async Task<HttpResponseMessage> CheckStatus<T>(this HttpResponseMessage response, Func<T, string> messageRetriever)
        {
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            var message = await GetErrorMessage(response, messageRetriever);
            throw new Exception($"Error: {response.StatusCode}, {message}");
        }

        private static async Task<string> GetErrorMessage<T>(HttpResponseMessage response, Func<T, string> messageRetriever)
        {
            var error = await response.Content.ReadAsStringAsync();
            try
            {
                return messageRetriever(JsonConvert.DeserializeObject<T>(error));
            }
            catch (JsonReaderException)
            {
                return error;
            }
        }
    }
}
