using Resiliency.Domain.Models;
using System.Threading.Tasks;

namespace Resiliency.Service.Online.Contracts.Services
{
    public interface IClientServiceOnline
    {
        Task<Client> SaveClientWithRetryPolicyAsync(Client obj);
        Task<Client> SaveClientWithFallbackPolicyAsync(Client obj);
        Task<Client> SaveClientWithWaitAndRetryPolicyAsync(Client obj);
        Task<Client> SaveClientWithCircuitBreakerAsync(Client obj);
    }
}
