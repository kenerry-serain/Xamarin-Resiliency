using Resiliency.Repository.Online.Models;
using System.Threading.Tasks;

namespace Resiliency.Repository.Online.Contracts.ClientHandler
{
    public interface IHttpResilientClient
    {
        Task<ApiResponse> DoCallAsyncWithRetryPolicy(ApiRequest request);
        Task<ApiResponse> DoCallAsyncWithFallbackPolicy(ApiRequest request);
        Task<ApiResponse> DoCallAsyncWithCircuitBreakerAsync(ApiRequest request);
        Task<ApiResponse> DoCallAsyncWithWaitAndRetryPolicy(ApiRequest request);
    }
}
