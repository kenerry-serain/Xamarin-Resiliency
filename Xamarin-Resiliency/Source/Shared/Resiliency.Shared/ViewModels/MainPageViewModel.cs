using Prism.Navigation;
using Resiliency.Shared.Abstractions;

namespace Resiliency.Shared.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel(IDialogService dialogService, INavigationService navigationService) : base(dialogService, navigationService)
        {
        }
    }
}
