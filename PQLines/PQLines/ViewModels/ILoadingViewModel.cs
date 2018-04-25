using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PQLines.ViewModels
{
    // Attach this interface to a View Model implementation that you want a loading activity indicator to be applied to
    public interface ILoadingViewModel : INotifyPropertyChanged, IViewModel
    {
        bool IsBusy { get; set; }
        void OnPropertyChanged([CallerMemberName] string propertyName = null);
    }
}