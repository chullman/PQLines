using System.Threading.Tasks;
using PQLines.ViewModels;

namespace PQLines.Services.PageNavigation
{
    public interface IViewModelNavigator
    {
        Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;

        // No need for these at the moment
        /*
        Task<IViewModel> PopAsync();
        Task PopToRootAsync();
        */ 
    }
}