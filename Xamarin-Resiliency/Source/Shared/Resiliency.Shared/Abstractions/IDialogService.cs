using System.Threading.Tasks;

namespace Resiliency.Shared.Abstractions
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message);
    }
}
