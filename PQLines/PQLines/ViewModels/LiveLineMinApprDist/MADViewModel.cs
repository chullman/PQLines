using System;
using System.Collections.Generic;
using System.Linq;
using PQLines.Models;
using PQLines.Services.PageNavigation;
using Xamarin.Forms;

namespace PQLines.ViewModels.ConductorInfo
{
    public class MADViewModel : INavigationViewModel
    {
        private readonly IList<string> _buttonTextsParentIDs = new List<string>();
        private readonly MADContent _deserializedContents;
        private readonly IViewModelNavigator _viewModelNavigator;
        private readonly IList<IContentViewModel> _viewModelsICanNavigateTo;

        public MADViewModel(IRootModel deserializedContents, IViewModelNavigator viewModelNavigator,
            IList<IContentViewModel> viewModelsICanNavigateTo)
        {
            _deserializedContents = deserializedContents as MADContent;
            _viewModelNavigator = viewModelNavigator;
            _viewModelsICanNavigateTo = viewModelsICanNavigateTo;

            // Find and grab the relevant "ID" fields from the Models as these will be used for the buttons' text
            if (_deserializedContents != null)
            {
                foreach (var category in _deserializedContents.Categories)
                {
                    _buttonTextsParentIDs.Add(category.ID);
                }
            }
            else
            {
                throw new ArgumentNullException("deserializedContents", String.Format("{0} is referencing null deserialized contents", GetType()));
            }

            Title = "Category";
        }

        public string Title { get; set; }

        // Used by the View to know how many buttons to display
        public int GetIDsCount()
        {
            if (_deserializedContents.Categories.Count != 0)
            {
                return _deserializedContents.Categories.Count;
            }
            throw new NullReferenceException(string.Format("ERROR: The queried data appears to be empty - check the query within View Model class {0}", GetType()));
        }

        // The command that is processed when a navigation button is clicked
        public Command NavigateToViewModel(int clickedButtonNum)
        {
            // Note that unlike using Application.Current.MainPage.Navigation.PushAsync(), we can pass in a view model instead of a page, as _ViewNavigator uses our ViewFactory implementation
            // This makes for much better view and view model coupling!
            return
                new Command(async () => await _viewModelNavigator.PushAsync(_viewModelsICanNavigateTo[clickedButtonNum]));
        }

        public string GetParentIDText(int buttonNum)
        {
            if (_buttonTextsParentIDs.Any())
            {
                return _buttonTextsParentIDs[buttonNum];
            }
            throw new NullReferenceException(string.Format("ERROR: No relevant 'ID' strings were found - check the query within View Model class {0}", GetType()));
        }
    }
}