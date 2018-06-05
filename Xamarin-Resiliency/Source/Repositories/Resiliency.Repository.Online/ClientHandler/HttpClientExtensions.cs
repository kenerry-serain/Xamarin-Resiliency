using Newtonsoft.Json;
using Resiliency.Domain.InMemory;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Resiliency.Repository.Online.ClientHandler
{
    public class HttpClientExtensions : HttpClient
    {
        readonly HttpClient _httpClient;
        public HttpClientExtensions()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(WebAPIRoutes.ApplicationBaseUrl)
            };
        }

        public void SetApplicationMediaType(string applicationType) =>
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationType));

        public void ClearHeaderValue() =>
            _httpClient.DefaultRequestHeaders.Clear();

        public static TOutput GetResponse<TOutput>(string textToDeserialize) where TOutput : class =>
            JsonConvert.DeserializeObject<TOutput>(textToDeserialize);
    }
}
