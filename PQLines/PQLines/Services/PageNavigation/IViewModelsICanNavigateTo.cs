using System.Collections.Generic;
using PQLines.ViewModels;

namespace PQLines.Services.PageNavigation
{
    public interface IViewModelsICanNavigateTo
    {
        void AddTo<T>(IContentViewModel navigationViewModel) where T : INavigationViewModel;
        IList<IContentViewModel> GetAllFrom<T>() where T : INavigationViewModel;
    }
}