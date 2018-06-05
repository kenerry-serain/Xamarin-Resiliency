using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Resiliency.Domain.InMemory;
using Resiliency.Repository.Online.Contracts.ClientHandler;
using Resiliency.Repository.Online.InMemory;
using Resiliency.Repository.Online.Models;
using Resiliency.Service.SignalR.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Resiliency.Repository.Online.ClientHandler
{
    public class HttpResilientClient : IHttpResilientClient
    {
        private readonly int _retryCount;
        private readonly int _durationOfbreak;
        private readonly int _handledEventsAllowedBeforeBreaking;
        private readonly HttpClient _invoker;
        private readonly int _sleepDurationSeconds;
        private readonly SignalRHelper _signalRHelper;
        public HttpResilientClient
        (
            int retryCount = 10,
            int sleepDurationSeconds = 5,
            int durationOfbreak = 3,
            int handledEventsAllowedBeforeBreaking = 1
        )
        {
            _invoker = new HttpClient { BaseAddress = new Uri(WebAPIRoutes.ApplicationBaseUrl) };
            _retryCount = retryCount;
            _durationOfbreak = durationOfbreak;
            _sleepDurationSeconds = sleepDurationSeconds;
            _handledEventsAllowedBeforeBreaking = handledEventsAllowedBeforeBreaking;
            _signalRHelper = new SignalRHelper();
        }

        /// <summary>
        /// Make a resilient request with WaitAndRetryPolicy
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse> DoCallAsyncWithWaitAndRetryPolicy(ApiRequest request)
        {
            var policy = WaitAndRetryPolicy();
            return await policy.ExecuteAsync(async () =>
                await CallAsync(request));
        }

        /// <summary>
        /// Make a resilient request with CircuitBreakerAsync
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse> DoCallAsyncWithCircuitBreakerAsync(ApiRequest request)
        {
            var policy = CircuitBreakerPolicy();
            var teste = await policy.ExecuteAsync(async () =>
                await CallAsync(request));
            policy.Reset();

            return teste;
        }

        /// <summary>
        /// Make a resilient request with RetryPolicy
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse> DoCallAsyncWithRetryPolicy(ApiRequest request)
        {
            var policy = RetryPolicy();
            return await policy.ExecuteAsync(async () =>
                await CallAsync(request));
        }

        /// <summary>
        /// Make a resilient request with FallbackPolicy
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse> DoCallAsyncWithFallbackPolicy(ApiRequest request)
        {
            var policy = FallbackPolicy();
            return await policy.ExecuteAsync(async () =>
                await CallAsync(request));
        }



        ///// <summary>
        ///// Make a resilient request with RetryPolicy
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //public async Task<ApiResponse> DoCallAsyncWithRetryAndFallbackPolicy(ApiRequest request)
        //{
        //    var policy = RetryPolicy();
        //    return await policy.ExecuteAsync(async () =>
        //        await CallAsync(request));
        //}


        private async Task<ApiResponse> CallAsync(ApiRequest request, bool throwsIfFails = true)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            /* Create a new Http Message, using the Verb and Uri */
            var message = new HttpRequestMessage(new HttpMethod(request.Verb), request.RequestUri);

            /* Sets the message content (String or UrlForm) and message header */
            AddMessageContent(request, message);
            AddMessageHeader(message, request.Headers);

            var response = await _invoker.SendAsync(message);
            var responseContent = await response.Content.ReadAsStringAsync();

            /* Do we have errors on the HTTP Response? */
            if (!response.IsSuccessStatusCode)
            {
                //if (throwsIfFails)
                //throw new HttpRequestException(response.ReasonPhrase);
            }

            return new ApiResponse
            {
                ResponseContent = responseContent,
                StatusCode = (int)response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }

        private CircuitBreakerPolicy<ApiResponse> CircuitBreakerPolicy()
        {
            var circuitBreaker= Policy<ApiResponse>
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .OrResult(IsNotSucessfulRequest)
                .CircuitBreakerAsync(
                    durationOfBreak: TimeSpan.FromSeconds(_durationOfbreak),
                    handledEventsAllowedBeforeBreaking: _handledEventsAllowedBeforeBreaking,
                    onBreak: async (result, timeSpan) =>
                    {
                        await NewMethod(result,timeSpan);
                    },
                    onReset: async () =>
                    {
                        await _signalRHelper.OpenChannel();
                        var formattedMessage ="CLOSING CIRCUIT! (CIRCUIT RESETED)";
                        await _signalRHelper.SendSignalRMessage("CircuitBreakerPolicy", formattedMessage);
                    });

            return circuitBreaker;
        }

        private async Task NewMethod(DelegateResult<ApiResponse> result, TimeSpan timeSpan)
        {
            await _signalRHelper.OpenChannel();
            var formattedMessage =
                            $" OPENED CIRCUIT (CIRCUIT BREAKED): {timeSpan} |" +
                            $" WAITING: {timeSpan} |" +
                            $" STATUSCODE: {result.Result.StatusCode} |" +
                            $" REASONPHRASE: {result.Result.ReasonPhrase} |";
            //$" RESPONSECONTENT: {result.Result.ResponseContent} |" +
            //$" EXCEPTION: {result.Exception}";

            await _signalRHelper.SendSignalRMessage("CircuitBreakerPolicy", formattedMessage);
        }

        private RetryPolicy<ApiResponse> WaitAndRetryPolicy()
        {
            return Policy<ApiResponse>
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .OrResult(IsNotSucessfulRequest)
                .WaitAndRetryAsync
                (
                    _retryCount,
                    retryAttempt => TimeSpan.FromSeconds(_sleepDurationSeconds),
                    async (result, timeSpan, retryCount, context) =>
                    {
                        await _signalRHelper.OpenChannel();
                        var formattedMessage =
                            $" WAITING: {timeSpan} |" +
                            $" RETRYCOUNT: {retryCount} |" +
                            $" STATUSCODE: {result.Result.StatusCode} |" +
                            $" REASONPHRASE: {result.Result.ReasonPhrase} |";
                        //$" RESPONSECONTENT: {result.Result.ResponseContent} |" +
                        //$" EXCEPTION: {result.Exception}";

                        await _signalRHelper.SendSignalRMessage("WaitAndRetryPolicy", formattedMessage);
                    }
                );
        }

        private RetryPolicy<ApiResponse> RetryPolicy()
        {
            return Policy<ApiResponse>
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .OrResult(IsNotSucessfulRequest)
                .RetryAsync(_retryCount, async (result, retryCount) =>
                {
                    await _signalRHelper.OpenChannel();
                    var formattedMessage =
                        $" RETRYCOUNT: {retryCount} |" +
                        $" STATUSCODE: {result.Result.StatusCode} |" +
                        $" REASONPHRASE: {result.Result.ReasonPhrase} |";
                    //$" RESPONSECONTENT: {result.Result.ResponseContent} |" +
                    //$" EXCEPTION: {result.Exception}";

                    await _signalRHelper.SendSignalRMessage("RetryPolicy", formattedMessage);
                });
        }

        private FallbackPolicy<ApiResponse> FallbackPolicy()
        {
            return Policy<ApiResponse>
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .OrResult(IsNotSucessfulRequest)
                .FallbackAsync(ExecuteFallBack, async result =>
                {
                    await _signalRHelper.OpenChannel();
                    var formattedMessage =
                        $" STATUSCODE: {result.Result.StatusCode} |" +
                        $" REASONPHRASE: {result.Result.ReasonPhrase} |";
                    //$" RESPONSECONTENT: {result.Result.ResponseContent} |" +
                    //$" EXCEPTION: {result.Exception}";

                    await _signalRHelper.SendSignalRMessage("FallbackPolicy", formattedMessage);
                });
        }

        private Task<ApiResponse> ExecuteFallBack(CancellationToken arg)
        {
            return Task.FromResult(new ApiResponse());
        }


        #region Helpers
        /// <summary>
        /// Adding request content 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="message"></param>
        private static void AddMessageContent(ApiRequest request, HttpRequestMessage message)
        {
            if (!string.IsNullOrWhiteSpace(request.JsonStringContent))
            {
                message.Content = new StringContent
                (
                    request.JsonStringContent,
                    Encoding.UTF8,
                    HttpConstants.ContentType.ApplicationJson
                );
                return;
            }

            if (request.FormContent != null && request.FormContent.Any())
                message.Content = new FormUrlEncodedContent(request.FormContent);
        }

        /// <summary>
        /// Adding header content
        /// </summary>
        /// <param name="message"></param>
        /// <param name="headers"></param>
        private static void AddMessageHeader(HttpRequestMessage message, IEnumerable<KeyValuePair<string, string>> headers)
        {
            if (headers == null)
                return;

            foreach (var keyValuePair in headers)
                message.Headers.Add(keyValuePair.Key, keyValuePair.Value);
        }

        private static bool IsNotSucessfulRequest(ApiResponse response)
        {
            /* IsSuccessStatusCode - 200-299  */
            return !response.IsSuccessStatusCode && !HandledHttpStatusCode.Contains(response.StatusCode);
        }

        private static List<int> HandledHttpStatusCode => new List<int>
        {
            //(int)HttpStatusCode.NotFound, // 404
            (int)HttpStatusCode.Unauthorized, // 401
            (int)HttpStatusCode.Conflict, // 409
            (int)HttpStatusCode.Forbidden, // 403
            (int)HttpStatusCode.BadRequest // 400

        };
        #endregion
    }
}
