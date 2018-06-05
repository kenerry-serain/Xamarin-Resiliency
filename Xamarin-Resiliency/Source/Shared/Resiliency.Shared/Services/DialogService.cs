using Prism.Services;
using Resiliency.Shared.Abstractions;
using System.Threading.Tasks;

namespace Resiliency.Shared.Services
{
    public class DialogService : IDialogService
    {
        private readonly IPageDialogService _pageDialogService;

        public DialogService(IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
        }

        public async Task ShowAlertAsync(string message)
        {
            await _pageDialogService.DisplayAlertAsync("Smart Message", $"\n{message}", "Ok");
        }
    }
}
