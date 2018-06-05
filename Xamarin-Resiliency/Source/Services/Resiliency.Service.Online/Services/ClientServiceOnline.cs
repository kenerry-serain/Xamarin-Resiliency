using Resiliency.Domain.Models;
using Resiliency.Repository.Online.ClientHandler;
using Resiliency.Repository.Online.Contracts.ClientHandler;
using Resiliency.Repository.Online.Repositories;
using Resiliency.Service.Online.Contracts.Services;

namespace Resiliency.Service.Online.Services
{
    public sealed class ClientServiceOnline : RepositoryBaseOnline<Client>, IClientServiceOnline
    {
        public ClientServiceOnline(HttpClientExtensions httpClient, IHttpResilientClient httpClientResilientClient)
            : base(httpClient, httpClientResilientClient)
        {
        }
    }
}
