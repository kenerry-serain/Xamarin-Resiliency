using Resiliency.Domain.Models;
using Resiliency.Repository.Online.ClientHandler;
using Resiliency.Repository.Online.Contracts.ClientHandler;
using Resiliency.Repository.Online.Contracts.Repositories;

namespace Resiliency.Repository.Online.Repositories
{
    public sealed class ClientRepositoryOnline : RepositoryBaseOnline<Client>, IClientRepositoryOnline
    {
        public ClientRepositoryOnline(HttpClientExtensions httpClient, IHttpResilientClient httpClientResilientClient)
            : base(httpClient, httpClientResilientClient)
        {
        }
    }
}
