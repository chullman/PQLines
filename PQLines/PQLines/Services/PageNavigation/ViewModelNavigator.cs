using System;
using System.Diagnostics;
using System.Threading.Tasks;
using PQLines.ViewModels;
using Xamarin.Forms;

namespace PQLines.Services.PageNavigation
{
    // This is our own page navigator service wrapped around Xamarin.Forms INavigation
    // This is so we can do our navigation with View Models (with thanks to our "view factory") instead of using the INavigation's implementation which relys on Views (pages) to navigate
    // With reference to: http://adventuresinxamarinforms.com/2014/11/21/creating-a-xamarin-forms-app-part-6-view-model-first-navigation/
    public class ViewModelNavigator : IViewModelNavigator
    {
        // Must be LAZY initialised as we need to initialise it only AFTER we have set the main page of the app back in App.cs
        // This is especially important as in App.cs, we need to resolve this class BEFORE we set the main page (i.e. the home page)
        // because we need to pass this navigator class into our home page View Model and into all subsequent View Models that do navigation
        //
        // I'm sure there's a better way of doing this?
        private readonly Lazy<INavigation> _navigation;

        private readonly IViewFactory _viewFactory;

        public ViewModelNavigator(Lazy<INavigation> navigation, IViewFactory viewFactory)
        {
            _navigation = navigation;
            _viewFactory = viewFactory;
        }

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel
        {
            var view = _viewFactory.CreateThemedContentPage(viewModel);

            try
            {
                await _navigation.Value.PushAsync(view);
            }
            catch (Exception)
            {
                Debug.WriteLine("ERROR: Xamarin.Forms' INavigation implementation was unable push the page onto the navigation stack - is the page a valid Xamarin page?");
                throw;
            }
            

            return viewModel;
        }

        // No need for these at the moment - the mobile devices handles the popping of views automatically for us
        // May need to use these methods however if the mobile devices aren't properly popping pages from the navigation stack
        /*
        public async Task<IViewModel> PopAsync()
        {
            var view = await _navigation.Value.PopAsync();
            return view.BindingContext as IViewModel;
        }

        public async Task PopToRootAsync()
        {
            await _navigation.Value.PopToRootAsync();
        }
        */ 
    }
}