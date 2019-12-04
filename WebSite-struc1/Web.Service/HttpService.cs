using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Common2;
using Common2.Interfaces;

namespace Web.Service
{
    internal class HttpService : IHttpService
    {
        protected readonly ILoggingService<HttpService> LoggingService;
        private readonly HttpClient _client;
        private readonly MediaTypeFormatter _formatter = new JsonMediaTypeFormatter();

        public HttpService(ILoggingService<HttpService> loggingService)
        {
            LoggingService = loggingService;
            var handler = new HttpClientHandler { UseDefaultCredentials = true };
            _client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromMinutes(10)
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Gets data from specified end point.
        /// </summary>
        /// <typeparam name="TResult">The type of the data to be returned.</typeparam>
        /// <param name="endPointTemplate">The template string for the end point.</param>
        /// <param name="args">The args to be substituted into the end point template.</param>
        /// <returns>The data.</returns>
        public Task<TResult> Get<TResult>(string endPointTemplate, params string[] args)
        {
            return SendRequest<TResult>(_client.GetAsync, endPointTemplate, args);
        }

        /// <summary>
        /// Deletes data from specified end point.
        /// </summary>
        /// <typeparam name="TResult">The type of the data to be returned.</typeparam>
        /// <param name="endPointTemplate">The template string for the end point.</param>
        /// <param name="args">The args to be substituted into the end point template.</param>
        /// <returns>The data.</returns>
        public Task<TResult> Delete<TResult>(string endPointTemplate, params string[] args)
        {
            return SendRequest<TResult>(_client.DeleteAsync, endPointTemplate, args);
        }

        /// <summary>
        /// Posts request data to specified end point.
        /// </summary>
        /// <typeparam name="TResult">The type of the result to be returned.</typeparam>
        /// <typeparam name="TObj">The type of the object being posted.</typeparam>
        /// <param name="requestData">The request data.</param>
        /// <param name="endPointTemplate">The template string for the end point.</param>
        /// <param name="args">The args to be substituted into the end point template.</param>
        /// <returns>The result of the request.</returns>
        public Task<TResult> Post<TResult, TObj>(TObj requestData, string endPointTemplate, params string[] args)
        {
            return SendRequest<TResult>(url => _client.PostAsync(url, new ObjectContent<TObj>(requestData, _formatter)), endPointTemplate, args);
        }

        /// <summary>
        /// Puts the specified request data.
        /// </summary>
        /// <typeparam name="TResult">The type of the result to be returned.</typeparam>
        /// <typeparam name="TObj">The type of the object being put.</typeparam>
        /// <param name="requestData">The request data.</param>
        /// <param name="endPointTemplate">The template string for the end point.</param>
        /// <param name="args">The args to be substituted into the end point template.</param>
        /// <returns>The result of the request.</returns>
        public Task<TResult> Put<TResult, TObj>(TObj requestData, string endPointTemplate, params string[] args)
        {
            return SendRequest<TResult>(url => _client.PutAsync(url, new ObjectContent<TObj>(requestData, _formatter)), endPointTemplate, args);
        }

        /// <summary>
        /// Sends the request to the specified end point.
        /// </summary>
        /// <typeparam name="T">The type of data to be received back.</typeparam>
        /// <param name="webMethod">The web method to call.</param>
        /// <param name="endPointTemplate">The template string for the end point.</param>
        /// <param name="args">The args to be substituted into the end point template.</param>
        /// <returns>The result of the request.</returns>
        private async Task<T> SendRequest<T>(Func<Uri, Task<HttpResponseMessage>> webMethod, string endPointTemplate, params string[] args)
        {
            var uri = FormatUri(endPointTemplate, args);
            var endPoint = uri.AbsoluteUri;
            LoggingService.Debug($"{nameof(SendRequest)}<{typeof(T)}>({endPoint})");

            T result;
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await webMethod(uri).Caf();
                stopwatch.Stop();
                LoggingService.Debug($"{endPoint} completed successfully in {stopwatch.ElapsedMilliseconds}ms");
                result = await ReadResult<T>(response).Caf();
            }
            catch (Exception ex)
            {
                if (stopwatch.IsRunning)
                {
                    stopwatch.Stop();
                }
                LoggingService.Error($"{endPoint} completed with errors in {stopwatch.ElapsedMilliseconds}ms");
                LoggingService.Error($"{ex.Message}\n{ex.StackTrace}");
                throw;
            }

            return result;
        }

        private static Uri FormatUri(string endPointTemplate, IEnumerable<string> args)
        {
            var endPoint = string.Format(endPointTemplate, args.Select(Uri.EscapeDataString).Cast<object>().ToArray());
            return new Uri(endPoint);
        }

        /// <summary>
        /// Reads the result from the given request.
        /// </summary>
        /// <typeparam name="T">The type of the result to be returned.</typeparam>
        /// <param name="responseMessage">The response message</param>
        /// <returns>The result of the given request.</returns>
        protected virtual async Task<T> ReadResult<T>(HttpResponseMessage responseMessage)
        {
            var statusResult = await responseMessage.CheckStatus();
            return await statusResult
                .Content
                .ReadAsAsync<T>();
        }
    }
}
