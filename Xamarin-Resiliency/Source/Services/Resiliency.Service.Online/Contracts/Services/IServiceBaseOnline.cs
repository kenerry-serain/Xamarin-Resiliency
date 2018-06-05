using System.Collections.Generic;
using System.Threading.Tasks;
using Resiliency.Domain.Models;

namespace Resiliency.Service.Online.Contracts.Services
{
    public interface IServiceBaseOnline<TEntity> where TEntity : Entity
    {
        //Queries
        Task<IEnumerable<TEntity>> GetAllAsync(string route);
        Task<TEntity> FindByIdAsync(string route, string primaryKey);

        //Commands
        Task<TEntity> AddAsync(string route, TEntity obj);
        Task DeleteAsync(string route, string primaryKey);
        Task<TEntity> UpdateAsync(string route, TEntity obj);
    }
}
