using Newtonsoft.Json;
using Resiliency.Domain.InMemory;
using Resiliency.Domain.Models;
using Resiliency.Repository.Online.ClientHandler;
using Resiliency.Repository.Online.Contracts.ClientHandler;
using Resiliency.Repository.Online.Contracts.Repositories;
using Resiliency.Repository.Online.InMemory;
using Resiliency.Repository.Online.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resiliency.Repository.Online.Repositories
{
    public abstract class RepositoryBaseOnline<TEntity> : IRepositoryBaseOnline<TEntity> where TEntity : Entity
    {
        private readonly HttpClientExtensions _httpClient;
        private readonly IHttpResilientClient _httpClientResilientClient;

        protected RepositoryBaseOnline(HttpClientExtensions httpClient, IHttpResilientClient httpClientResilientClient)
        {
            _httpClient = httpClient;
            _httpClientResilientClient = httpClientResilientClient;
            _httpClient.ClearHeaderValue();
            _httpClient.SetApplicationMediaType(WebAPIKeys.ApplicationMediaType);
            //_httpClient.SetApplicationAuthorization(TokenType.AccessToken, WebAPIKeys.CurrentToken);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(string route)
        {
            try
            {
                var request = new ApiRequest
                {
                    RequestUri = route,
                    Verb = HttpConstants.Verb.Get
                };
                var response = await _httpClientResilientClient.DoCallAsyncWithWaitAndRetryPolicy(request).ConfigureAwait(false);
                var result = HttpClientExtensions.GetResponse<IEnumerable<TEntity>>(response.ResponseContent);
                return result;
            }
            catch (Exception exception)
            {
                throw new HttpRequestException(exception.Message);
            }
        }

        public async Task<TEntity> FindByIdAsync(string route, string primaryKey)
        {
            try
            {
                var completeRoute = $"{route}/{primaryKey}";
                var request = new ApiRequest
                {
                    RequestUri = completeRoute,
                    Verb = HttpConstants.Verb.Get
                };
                var response = await _httpClientResilientClient.DoCallAsyncWithWaitAndRetryPolicy(request).ConfigureAwait(false);
                var result = HttpClientExtensions.GetResponse<TEntity>(response.ResponseContent);
                return result;
            }
            catch (Exception exception)
            {
                throw new HttpRequestException(exception.Message);
            }
        }
        public async Task<TEntity> AddAsync(string route, TEntity obj)
        {
            try
            {
                var stringContent = JsonConvert.SerializeObject(obj);
                var request = new ApiRequest
                {
                    RequestUri = route,
                    Verb = HttpConstants.Verb.Post,
                    JsonStringContent = stringContent

                };
                var response = await _httpClientResilientClient.DoCallAsyncWithWaitAndRetryPolicy(request).ConfigureAwait(false);
                var result = HttpClientExtensions.GetResponse<TEntity>(response.ResponseContent);
                return result;
            }
            catch (Exception exception)
            {
                throw new HttpRequestException(exception.Message);
            }
        }

        public async Task<TEntity> UpdateAsync(string route, TEntity obj)
        {
            try
            {
                var stringContent = JsonConvert.SerializeObject(obj);
                var request = new ApiRequest
                {
                    RequestUri = route,
                    Verb = HttpConstants.Verb.Put,
                    JsonStringContent = stringContent

                };
                var response = await _httpClientResilientClient.DoCallAsyncWithWaitAndRetryPolicy(request).ConfigureAwait(false);
                var result = HttpClientExtensions.GetResponse<TEntity>(response.ResponseContent);
                return result;
            }
            catch (Exception exception)
            {
                throw new HttpRequestException(exception.Message);
            }
        }

        public async Task DeleteAsync(string route, string primaryKey)
        {
            try
            {
                var completeRoute = $"{route}/{primaryKey}";
                var request = new ApiRequest
                {
                    RequestUri = completeRoute,
                    Verb = HttpConstants.Verb.Delete
                };
                await _httpClientResilientClient.DoCallAsyncWithWaitAndRetryPolicy(request).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                throw new HttpRequestException(exception.Message);
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
        }

        private static bool IsNotSucessfulRequest(ApiResponse response)
        {
            /* IsSuccessStatusCode - 200-299  */
            return !response.IsSuccessStatusCode && !HandledHttpStatusCode.Contains(response.StatusCode);
        }

        private static List<int> HandledHttpStatusCode => new List<int>
        {
            (int)HttpStatusCode.NotFound, // 404
            (int)HttpStatusCode.Unauthorized, // 401
            (int)HttpStatusCode.Conflict, // 409
            (int)HttpStatusCode.Forbidden, // 403
            (int)HttpStatusCode.BadRequest // 400
        };
        //private static void TreatException(ApiResponse apiResponse)
        //{
        //    if (apiResponse.StatusCode == (int)HttpStatusCode.Unauthorized)
        //        throw new HttpRequestException("401 - Usuário não autorizado");

        //    throw new Exception(apiResponse.ReasonPhrase);
        //}
    }
}
