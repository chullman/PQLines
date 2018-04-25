using PQLines.ViewModels;
using Xamarin.Forms;

namespace PQLines.Services.PageNavigation
{
    public interface IViewFactory
    {
        void Map<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Page;

        // Should be all we'll ever need
        Page CreateThemedNavPage<TViewModel>(TViewModel viewModel) where TViewModel : class, INavigationViewModel;
        Page CreateThemedContentPage<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;
        Page CreatePage<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel;
    }
}