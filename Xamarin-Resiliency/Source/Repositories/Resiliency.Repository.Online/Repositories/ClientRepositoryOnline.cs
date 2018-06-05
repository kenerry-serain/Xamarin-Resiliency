using Newtonsoft.Json;
using Resiliency.Domain.InMemory;
using Resiliency.Domain.Models;
using Resiliency.Repository.Online.ClientHandler;
using Resiliency.Repository.Online.Contracts.ClientHandler;
using Resiliency.Repository.Online.Contracts.Repositories;
using Resiliency.Repository.Online.InMemory;
using Resiliency.Repository.Online.Models;
using System.Threading.Tasks;

namespace Resiliency.Repository.Online.Repositories
{
    public sealed class ClientRepositoryOnline : RepositoryBaseOnline<Client>, IClientRepositoryOnline
    {
        private readonly IHttpResilientClient _httpClientResilientClient;
        public ClientRepositoryOnline(HttpClientExtensions httpClient, IHttpResilientClient httpClientResilientClient)
            : base(httpClient, httpClientResilientClient)
        {
            _httpClientResilientClient = httpClientResilientClient;
        }

        public async Task<Client> SaveClientWithRetryPolicyAsync(Client obj)
        {
            var stringContent = JsonConvert.SerializeObject(obj);
            var request = new ApiRequest
            {
                RequestUri = WebAPIRoutes.RegisterClient,
                Verb = HttpConstants.Verb.Post,
                JsonStringContent = stringContent

            };
            var response = await _httpClientResilientClient.DoCallAsyncWithRetryPolicy(request);
            var result = HttpClientExtensions.GetResponse<Client>(response.ResponseContent);
            return result;
        }

        public async Task<Client> SaveClientWithFallbackPolicyAsync(Client obj)
        {
            var stringContent = JsonConvert.SerializeObject(obj);
            var request = new ApiRequest
            {
                RequestUri = WebAPIRoutes.RegisterClient,
                Verb = HttpConstants.Verb.Post,
                JsonStringContent = stringContent

            };
            var response = await _httpClientResilientClient.DoCallAsyncWithFallbackPolicy(request);
            var result = HttpClientExtensions.GetResponse<Client>(response.ResponseContent);
            return result;
        }

        public async Task<Client> SaveClientWithCircuitBreakerAsync(Client obj)
        {
            var stringContent = JsonConvert.SerializeObject(obj);
            var request = new ApiRequest
            {
                RequestUri = WebAPIRoutes.RegisterClient,
                Verb = HttpConstants.Verb.Post,
                JsonStringContent = stringContent

            };
            var response = await _httpClientResilientClient.DoCallAsyncWithCircuitBreakerAsync(request);
            var result = HttpClientExtensions.GetResponse<Client>(response.ResponseContent);
            return result;
        }

        public async Task<Client> SaveClientWithWaitAndRetryPolicyAsync(Client obj)
        {
            var stringContent = JsonConvert.SerializeObject(obj);
            var request = new ApiRequest
            {
                RequestUri = WebAPIRoutes.RegisterClient,
                Verb = HttpConstants.Verb.Post,
                JsonStringContent = stringContent

            };
            var response = await _httpClientResilientClient.DoCallAsyncWithWaitAndRetryPolicy(request);
            var result = HttpClientExtensions.GetResponse<Client>(response.ResponseContent);
            return result;
        }
    }
}
