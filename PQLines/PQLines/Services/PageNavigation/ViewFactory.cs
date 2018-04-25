using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Autofac;
using PQLines.ViewModels;
using PQLines.Views;
using Xamarin.Forms;

namespace PQLines.Services.PageNavigation
{
    // Our View Models need to know which Views its buttons when clicked will navigate to, however,
    //  in good MVVM design, we don't want any of our View Models to be dependent on any View
    // This ViewFactory class resolves that problem by handling the "mapping" of Views to View Models, and also finding and returning the View when given a View Model
    // Note that this View Factory only allows a 1-to-1 mapping of a View to View Model (i.e. one View per View Model)
    // With reference to: http://adventuresinxamarinforms.com/2014/11/21/creating-a-xamarin-forms-app-part-6-view-model-first-navigation/
    // and with reference to: https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/src/Forms/XLabs.Forms/Mvvm/ViewFactory.cs

    public class ViewFactory : IViewFactory
    {
        private readonly ILifetimeScope _autofacContainerScope;

        // Stores our 1-to-1 mapping of a View Model type to a View type
        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();

        public ViewFactory(ILifetimeScope scope)
        {
            _autofacContainerScope = scope;
        }

        public void Map<TViewModel, TView>() where TViewModel : class, IViewModel where TView : Page
        {
            _map[typeof (TViewModel)] = typeof (TView);
        }

        // Given a mapped View Model, creates a themed "Navigation Page" (with a themed Content Page inside that's binded to the View Model)
        public Page CreateThemedNavPage<TViewModel>(TViewModel viewModel) where TViewModel : class, INavigationViewModel
        {
            Type viewType;
            try
            {
                viewType = _map[viewModel.GetType()];
            }
            catch (Exception)
            {
                Debug.WriteLine("ERROR: Unable to retrieve Navigation Page from View Model - has it been mapped to the View Factory?");
                throw;
            }
            
            var view = _autofacContainerScope.Resolve(viewType, new TypedParameter(typeof(IContentViewModel), viewModel)) as Page;
            var navView = _autofacContainerScope.Resolve<ThemedNavigationPage>(new TypedParameter(typeof (Page), view));

            return navView;
        }


        // Given a mapped View Model, creates a themed "Content Page" that's binded to the View Model
        public Page CreateThemedContentPage<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel
        {
            Type viewType;

            try
            {
                viewType = _map[viewModel.GetType()];
            }
            catch (Exception)
            {
                Debug.WriteLine("ERROR: Unable to retrieve Content Page from View Model - has it been mapped to the View Factory?");
                throw;
            }

            var contView = _autofacContainerScope.Resolve(viewType, new TypedParameter(typeof(IContentViewModel), viewModel)) as ThemedContentPage;

            return contView;
        }

        // Currently not used, as currently all our pages (Views) are inherited under a "themed" page
        public Page CreatePage<TViewModel>(TViewModel viewModel) where TViewModel : class, IViewModel
        {
            Type viewType;
            try
            {
                viewType = _map[viewModel.GetType()];
            }
            catch (Exception)
            {
                Debug.WriteLine("ERROR: Unable to retrieve Page from View Model - has it been mapped to the View Factory?");
                throw;
            }

            var view = _autofacContainerScope.Resolve(viewType, new TypedParameter(typeof(IContentViewModel), viewModel)) as Page;

            return view;
        }
    }
}