using Prism;
using Prism.Autofac;
using Prism.Ioc;
using Resiliency.Repository.Online.ClientHandler;
using Resiliency.Repository.Online.Contracts.ClientHandler;
using Resiliency.Repository.Online.Contracts.Repositories;
using Resiliency.Repository.Online.Repositories;
using Resiliency.Service.Online.Contracts.Services;
using Resiliency.Service.Online.Services;
using Resiliency.Shared.Abstractions;
using Resiliency.Shared.Services;
using Resiliency.Shared.ViewModels;
using Resiliency.Shared.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Resiliency.Shared
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/AddClientPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<AddClientPage, AddClientPageViewModel>();

            containerRegistry.Register<IDialogService, DialogService>();
            containerRegistry.Register<IHttpResilientClient, HttpResilientClient>();
            containerRegistry.Register<IClientServiceOnline, ClientServiceOnline>();
            containerRegistry.Register<IClientRepositoryOnline, ClientRepositoryOnline>();
        }
    }
}
