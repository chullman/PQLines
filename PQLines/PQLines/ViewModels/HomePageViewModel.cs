using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using PQLines.Models;
using PQLines.Services.PageNavigation;
using Xamarin.Forms;

namespace PQLines.ViewModels
{
    // Source: http://adventuresinxamarinforms.com/2014/11/17/creating-a-xamarin-forms-application-part-4-dependency-injection/

    public class HomePageViewModel : INavigationViewModel, ILoadingViewModel
    {
        private readonly IList<string> _buttonTextsParentIDs = new List<string>();
        private readonly IList<IRootModel> _deserializedContents;
        private readonly IViewModelNavigator _viewModelNavigator;
        private bool _isBusy;
        private IList<IContentViewModel> _viewModelsICanNavigateTo;

        public HomePageViewModel(IList<IRootModel> deserializedContents, IViewModelNavigator viewModelNavigator)
        {
            _deserializedContents = deserializedContents;

            _viewModelNavigator = viewModelNavigator;


            foreach (var content in _deserializedContents)
            {
                _buttonTextsParentIDs.Add(content.Description);
            }


            Title = "Contents (Home)";
        }

        public string Title { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        // This bool is unique to this Home Page View Model
        // As this should be the first page that this app loads on launch,
        // we want to display a loading activity indicator to prevent further user activity until this flag is set to false
        // With reference to: http://forums.xamarin.com/discussion/33042/notify-a-method-when-binding-changes
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Used by the View to know how many buttons to display
        public int GetIDsCount()
        {
            if (_deserializedContents.Count != 0)
            {
                return _deserializedContents.Count;
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

        public void SetViewModelsICanNavigateTo(IList<IContentViewModel> viewModelsICanNavigateTo)
        {
            _viewModelsICanNavigateTo = viewModelsICanNavigateTo;
        }

        public string GetParentIDText(int buttonNum)
        {
            if (_buttonTextsParentIDs.Any())
            {
                return _buttonTextsParentIDs[buttonNum];
            }
            throw new NullReferenceException(string.Format("ERROR: No relevant 'Description' strings were found - check the query within View Model class {0}", GetType()));
        }
    }
}