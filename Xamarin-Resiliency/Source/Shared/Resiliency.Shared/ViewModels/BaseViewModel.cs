using Prism.Mvvm;
using Prism.Navigation;
using Resiliency.Shared.Abstractions;

namespace Resiliency.Shared.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware
    {
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;
        public BaseViewModel(IDialogService dialogService, INavigationService navigationService)
        {
            DialogService = dialogService;
            NavigationService = navigationService;
        }


        /// <summary>
        /// Each view can implement this method on it's way
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        /// <summary>
        /// Each view can implement this method on it's way
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        /// <summary>
        /// Each view can implement this method on it's way
        /// </summary>
        /// <param name="parameters"></param>
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {

        }
    }
}
