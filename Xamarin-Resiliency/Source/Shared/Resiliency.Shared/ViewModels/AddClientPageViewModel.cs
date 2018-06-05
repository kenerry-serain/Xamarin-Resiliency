using Prism.Commands;
using Prism.Navigation;
using Resiliency.Domain.Models;
using Resiliency.Service.Online.Contracts.Services;
using Resiliency.Shared.Abstractions;
using System.Threading.Tasks;

namespace Resiliency.Shared.ViewModels
{
    public class AddClientPageViewModel : BaseViewModel
    {
        private readonly IClientServiceOnline _resilientClient;
        public AddClientPageViewModel(IClientServiceOnline resilientClient, IDialogService dialogService, INavigationService navigationService)
            : base(dialogService, navigationService)
        {
            _resilientClient = resilientClient;
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        /// <summary>
        /// Save Client With Circuit Breaker Policy
        /// </summary>
        private DelegateCommand _saveClientWithCircuitBreakerAsyncCommand;
        public DelegateCommand SaveClientWithCircuitBreakerAsyncCommand =>
            _saveClientWithCircuitBreakerAsyncCommand ?? (_saveClientWithCircuitBreakerAsyncCommand = new DelegateCommand(async () => await ExecuteSaveClientAsyncWithCircuitBreakerAsync()));


        /// <summary>
        /// Save Client With Fallback Policy
        /// </summary>
        private DelegateCommand _saveClientWithFallbackPolicyAsyncCommand;
        public DelegateCommand SaveClientWithFallbackPolicyAsyncCommand =>
            _saveClientWithFallbackPolicyAsyncCommand ?? (_saveClientWithFallbackPolicyAsyncCommand = new DelegateCommand(async () => await ExecuteSaveClientWithFallbackPolicyAsync()));


        /// <summary>
        /// Save Client With Retry Policy
        /// </summary>
        private DelegateCommand _saveClientWithRetryPolicyCommand;
        public DelegateCommand SaveClientWithRetryPolicy =>
            _saveClientWithRetryPolicyCommand ?? (_saveClientWithRetryPolicyCommand = new DelegateCommand(async () => await ExecuteSaveClientWithRetryPolicyAsync()));


        /// <summary>
        /// Save Client With Wait And Retry Policy
        /// </summary>
        private DelegateCommand _saveClientWithWaitAndRetryPolicyAsync;
        public DelegateCommand SaveClientWithWaitAndRetryPolicyAsyncCommand =>
            _saveClientWithWaitAndRetryPolicyAsync ?? (_saveClientWithWaitAndRetryPolicyAsync = new DelegateCommand(async () => await ExecuteSaveClientWithWaitAndRetryPolicyAsync()));


        /// <summary>
        /// Execute Save Client With Circuit Breaker Policy
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteSaveClientAsyncWithCircuitBreakerAsync()
        {
            var client = new Client(Name, Email);
            await _resilientClient.SaveClientWithCircuitBreakerAsync(client);
        }


        /// <summary>
        /// Execute Save Client With Fallback Policy
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteSaveClientWithFallbackPolicyAsync()
        {
            var client = new Client(Name, Email);
            await _resilientClient.SaveClientWithFallbackPolicyAsync(client);
        }


        /// <summary>
        /// Execute Save Client With Retry Policy
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteSaveClientWithRetryPolicyAsync()
        {
            var client = new Client(Name, Email);
            await _resilientClient.SaveClientWithRetryPolicyAsync(client);
        }


        /// <summary>
        /// Execute Save Client With Wait And Retry Policy
        /// </summary>
        /// <returns></returns>
        private async Task ExecuteSaveClientWithWaitAndRetryPolicyAsync()
        {
            var client = new Client(Name, Email);
            await _resilientClient.SaveClientWithWaitAndRetryPolicyAsync(client);
        }
    }
}
