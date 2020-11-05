using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AGL.Services.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AGL.Services.Implementation
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpService> _logger;

        public HttpService(IHttpClientFactory httpClientFactory, ILogger<HttpService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<T> RetrieveJsonDataAsync<T>(string clientName)
        {
            try
            {
                HttpRequestMessage httpRequest = new HttpRequestMessage() { Method = HttpMethod.Get };

                var httpClient = _httpClientFactory.CreateClient(clientName);
                var httpResponse = await httpClient.SendAsync(httpRequest);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    string errorMessage = "Unable to retrieve data.";
                    throw new Exception(errorMessage);
                }

                string jsonData = await httpResponse.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }
    }
}
