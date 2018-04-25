using Autofac;
using PQLines.Services.ContentProvider;
using PQLines.Services.DataDeserializer;
using PQLines.Services.DataReader;
using PQLines.Services.PageNavigation;
using Xamarin.Forms;

namespace PQLines.Bootstrapping.Registration
{
    public class StartupServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register all shared services that are required to run at application startup

            builder.RegisterType<JsonDeserializer>().As<IDeserializer>().SingleInstance();
            builder.RegisterGeneric(typeof (JsonResourceContentProvider<>)).As(typeof (IContentProvider<>)).SingleInstance();
            builder.RegisterType<EmbeddedStreamProvider>().As<IStreamProvider>().SingleInstance();
            builder.RegisterType<ViewFactory>().As<IViewFactory>().SingleInstance();
            builder.RegisterType<ViewModelNavigator>().As<IViewModelNavigator>().SingleInstance();
            builder.RegisterType<ViewModelsICanNavigateTo>().As<IViewModelsICanNavigateTo>().InstancePerLifetimeScope();

            // Using delegate registration
            // From http://adventuresinxamarinforms.com/2014/11/21/creating-a-xamarin-forms-app-part-6-view-model-first-navigation/ 
            //      "This returns the App.Current.MainPage.Navigation when resolved by the Lazy<INavigation> in the Navigator" (the ViewModelNavigator class, in our case)
            builder.Register(context => Application.Current.MainPage.Navigation).SingleInstance();

        }
    }
}