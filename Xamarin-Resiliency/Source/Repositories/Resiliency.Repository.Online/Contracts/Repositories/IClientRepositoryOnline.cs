using Resiliency.Domain.Models;
using System.Threading.Tasks;

namespace Resiliency.Repository.Online.Contracts.Repositories
{
    public interface IClientRepositoryOnline : IRepositoryBaseOnline<Client>
    {
        Task<Client> SaveClientWithRetryPolicyAsync(Client obj);
        Task<Client> SaveClientWithFallbackPolicyAsync(Client obj);
        Task<Client> SaveClientWithCircuitBreakerAsync(Client obj);
        Task<Client> SaveClientWithWaitAndRetryPolicyAsync(Client obj);
    }
}
