using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Web.Service
{
    public class HttpPaasClient
    {

        /// <summary>
        /// Authorization Redirect Uri
        /// </summary>
        private readonly string _redirectUri = "http://local.token";

        /// <summary>
        /// Login Url for Authorization on Paas
        /// </summary>
        private string GetLoginUrl => $"{_authUri}&redirect_uri={_redirectUri}";
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;
        private readonly string _authUri;
        private HttpPaasAuthToken _httpPaasAuthToken;

        /// <summary>
        /// Constructor for HttpPaasClient
        /// </summary>
        /// <param name="authUri">Authentication URL (PingFed)</param>
        /// <param name="baseUri">Base URI</param>
        public HttpPaasClient(string authUri, string baseUri)
        {
            var clientHandler = new HttpClientHandler
            {
                UseDefaultCredentials = true,
                AllowAutoRedirect = false,
                Credentials = CredentialCache.DefaultCredentials
            };

            _httpClient = new HttpClient(clientHandler)
            {
                DefaultRequestHeaders =
                {
                    Accept = {new MediaTypeWithQualityHeaderValue("application/json")},
                    Connection = {"Keep-Alive"}
                }
            };

            _httpClient.DefaultRequestHeaders.Add("X-Application-Name", Constants.ApplicationName);
            _httpClient.DefaultRequestHeaders.Add("X-Correlation-ID", Guid.NewGuid().ToString());


            _baseUri = baseUri;
            _authUri = authUri;
        }

        private HttpPaasAuthToken GetAuthToken()
        {
            using (var pingFedHttpResponseMessage = _httpClient.GetAsync(GetLoginUrl).Result)
            {
                if (pingFedHttpResponseMessage.StatusCode == HttpStatusCode.Redirect)
                {
                    using (var tokenRequestHttpResponseMessage =
                        _httpClient.GetAsync(pingFedHttpResponseMessage.Headers.Location).Result)
                    {
                        if (tokenRequestHttpResponseMessage.StatusCode == HttpStatusCode.Redirect)
                            return new HttpPaasAuthToken(tokenRequestHttpResponseMessage.Headers.Location.ToString());

                        throw new HttpRequestException(
                            $"Error during Authorization process with {pingFedHttpResponseMessage.Headers.Location} => Response Code {tokenRequestHttpResponseMessage.StatusCode} - {tokenRequestHttpResponseMessage.ReasonPhrase}");
                    }
                }

                throw new HttpRequestException(
                    $"Error during Authorization process with {_authUri} => Response Code {pingFedHttpResponseMessage.StatusCode} - {pingFedHttpResponseMessage.ReasonPhrase}");

            }
        }


        /// <summary>
        /// Get Request to Paas Service
        /// for full URL uses concatenation with baseUri 
        /// </summary>
        /// <param name="relativeUri">relativeUri</param>
        /// <returns>requested data from Paas service</returns>
        public async Task<TResult> Get<TResult>(string relativeUri)
        {
            AuthorizeHttpClient();

            using (var response = await _httpClient.GetAsync($"{_baseUri}{relativeUri}"))
            {
                if (response.StatusCode == HttpStatusCode.OK)
                    return await response.Content.ReadAsAsync<TResult>();

                throw new HttpRequestException(
                    $"Error occurred during receiving data from {relativeUri} => Response Code {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        public async Task<TResult> Post<TResult, TObj>(string relativeUri, TObj value)
        {
            return await SendAsync<TResult, TObj>(relativeUri, value, HttpMethod.Post);
        }
        public async Task<TResult> Patch<TResult, TObj>(string relativeUri, TObj value)
        {
            return await SendAsync<TResult, TObj>(relativeUri, value, new HttpMethod("PATCH"));
        }
        public async Task<TResult> Put<TResult, TObj>(string relativeUri, TObj value)
        {
            return await SendAsync<TResult, TObj>(relativeUri, value, HttpMethod.Put);
        }

        public async Task Delete(string relativeUri)
        {
            AuthorizeHttpClient();

            using (var response = await _httpClient.DeleteAsync($"{_baseUri}{relativeUri}"))
            {
                if (response.IsSuccessStatusCode)
                    return;

                throw new HttpRequestException(
                    $"Error occurred during receiving data from {relativeUri} => Response Code {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        private async Task<TResult> SendAsync<TResult, TObj>(string relativeUri, TObj value, HttpMethod method)
        {
            AuthorizeHttpClient();

            var formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                }
            };

            var request = new HttpRequestMessage(method, new Uri($"{_baseUri}{relativeUri}"))
            {
                Content = new ObjectContent<TObj>(value, formatter, "application/json")
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsAsync<TResult>();

                throw new HttpRequestException(
                    $"Error occurred during receiving data from {relativeUri} => Response Code {response.StatusCode} - {response.ReasonPhrase}: {await response.Content.ReadAsStringAsync()}");
            }
        }
        private void AuthorizeHttpClient()
        {
            if (_httpPaasAuthToken != null && !_httpPaasAuthToken.Expired()) return;
            _httpPaasAuthToken = GetAuthToken();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(_httpPaasAuthToken.TokenType, _httpPaasAuthToken.AccessToken);
        }
    }
}
