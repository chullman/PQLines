using Xamarin.Forms;

namespace PQLines.ViewModels
{
    // View Model for navigation pages

    public interface INavigationViewModel : IContentViewModel
    {
        Command NavigateToViewModel(int clickedButtonNum);
    }
}