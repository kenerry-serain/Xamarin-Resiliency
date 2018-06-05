using Resiliency.Domain.Models;
using Resiliency.Repository.Online.ClientHandler;
using Resiliency.Repository.Online.Contracts.ClientHandler;
using Resiliency.Repository.Online.Contracts.Repositories;
using Resiliency.Repository.Online.Repositories;
using Resiliency.Service.Online.Contracts.Services;
using System.Threading.Tasks;

namespace Resiliency.Service.Online.Services
{
    public sealed class ClientServiceOnline : RepositoryBaseOnline<Client>, IClientServiceOnline
    {
        private readonly IClientRepositoryOnline _clientRepositoryOnline;
        public ClientServiceOnline(IClientRepositoryOnline clientRepositoryOnline, HttpClientExtensions httpClient, IHttpResilientClient httpClientResilientClient)
            : base(httpClient, httpClientResilientClient)
        {
            _clientRepositoryOnline = clientRepositoryOnline;
        }

        public async Task<Client> SaveClientWithRetryPolicyAsync(Client obj)
        {
            return await _clientRepositoryOnline.SaveClientWithRetryPolicyAsync(obj);
        }

        public async Task<Client> SaveClientWithFallbackPolicyAsync(Client obj)
        {
            return await _clientRepositoryOnline.SaveClientWithFallbackPolicyAsync(obj);
        }

        public async Task<Client> SaveClientWithWaitAndRetryPolicyAsync(Client obj)
        {
            return await _clientRepositoryOnline.SaveClientWithWaitAndRetryPolicyAsync(obj);
        }

        public async Task<Client> SaveClientWithCircuitBreakerAsync(Client obj)
        {
            return await _clientRepositoryOnline.SaveClientWithCircuitBreakerAsync(obj);
        }
    }
}
