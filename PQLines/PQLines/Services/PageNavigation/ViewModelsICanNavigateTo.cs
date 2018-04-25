using System;
using System.Collections.Generic;
using PQLines.ViewModels;

namespace PQLines.Services.PageNavigation
{
    // Stores the collection of View Models that any given View Model can navigate to - i.e. the View Models that any given display's buttons can navigate to when clicked
    // With reference to: http://stackoverflow.com/a/17887699

    public class ViewModelsICanNavigateTo : IViewModelsICanNavigateTo
    {
        // 1 View Model is able to navigate to 1 or MANY other View Models
        private readonly Dictionary<Type, List<IContentViewModel>> _viewModelsICanNavigateTo;

        public ViewModelsICanNavigateTo()
        {
            _viewModelsICanNavigateTo = new Dictionary<Type, List<IContentViewModel>>();
        }

        public void AddTo<T>(IContentViewModel navigationViewModel) where T : INavigationViewModel
        {
            if (_viewModelsICanNavigateTo.ContainsKey(typeof (T)))
            {
                var existingListValue = _viewModelsICanNavigateTo[typeof (T)];
                if (!existingListValue.Contains(navigationViewModel))
                {
                    existingListValue.Add(navigationViewModel);
                }
            }
            else if (!_viewModelsICanNavigateTo.ContainsKey(typeof (T)))
            {
                IList<IContentViewModel> newListValue = new List<IContentViewModel>();
                newListValue.Add(navigationViewModel);
                _viewModelsICanNavigateTo.Add(typeof (T), (List<IContentViewModel>) newListValue);
            }
        }

        // Returns all View Models mapped to any given View Model
        // (i.e. calling this method, e.g. "GetAllFrom<HomePageViewModel>()" should return the 5 View Models it is able to navigate to since the Home Page currently has 5 buttons)
        public IList<IContentViewModel> GetAllFrom<T>() where T : INavigationViewModel
        {
            if (_viewModelsICanNavigateTo.ContainsKey(typeof (T)))
            {
                return _viewModelsICanNavigateTo[typeof (T)];
            }
            throw new NullReferenceException(string.Format("ERROR: The collection of pages that {0} can navigate to is empty - did you call any .AddTo methods for this type?", typeof(T)));
        }
    }
}