using Resiliency.Domain.Models;
using Resiliency.Repository.Online.Contracts.Repositories;
using Resiliency.Service.Online.Contracts.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resiliency.Service.Online.Services
{
    public abstract class ServiceBaseOnline<TEntity> : IServiceBaseOnline<TEntity> where TEntity : Entity
    {
        private readonly IRepositoryBaseOnline<TEntity> _repositoryBaseOnline;
        protected ServiceBaseOnline(IRepositoryBaseOnline<TEntity> repositoryBaseOnline)
        {
            _repositoryBaseOnline = repositoryBaseOnline;
        }
        public Task<IEnumerable<TEntity>> GetAllAsync(string route)
        {
            return _repositoryBaseOnline.GetAllAsync(route);
        }
        public Task<TEntity> FindByIdAsync(string route, string primaryKey)
        {
            return _repositoryBaseOnline.FindByIdAsync(route, primaryKey);
        }

        public Task<TEntity> AddAsync(string route, TEntity obj)
        {
            return _repositoryBaseOnline.AddAsync(route, obj);
        }

        public Task<TEntity> UpdateAsync(string route, TEntity obj)
        {
            return _repositoryBaseOnline.UpdateAsync(route, obj);
        }
        public Task DeleteAsync(string route, string primaryKey)
        {
            return _repositoryBaseOnline.DeleteAsync(route, primaryKey);
        }
    }
}
