using Autofac;
using PQLines.Views;
using PQLines.Views.ConductorInfo;
using PQLines.Views.ConductorInfo.Conductors;
using PQLines.Views.ConductorInfo.Conductors.Measurements;
using PQLines.Views.ExclusionZones;
using PQLines.Views.LiveLineMinApprDist;
using PQLines.Views.LiveLineMinApprDist.Categories;
using PQLines.Views.LiveLineWorkingLdLmts;
using PQLines.Views.PlantAndVehicleApprDist;
using PQLines.Views.PlantAndVehicleApprDist.ApprTypes;
using PQLines.Views.PlantAndVehicleApprDist.ApprTypes.ObserverStates;

namespace PQLines.Bootstrapping.Registration
{
    public class ViewModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register all Views
            // Ideally they should all be registered as single instances, 
            // but just to be safe as we're changing the Views' to View Model binding context depending on what is passed into the view factory,
            // and because I haven't checked how they get disposed,
            // I'll keep these as instance per dependency (except for the theming views)

            // (shared/themed views)
            builder.RegisterType<ThemedNavigationPage>().SingleInstance();
            builder.RegisterType<ThemedContentPage>().SingleInstance();

            builder.RegisterType<HomePage>();

            // "Conductor Info"
            builder.RegisterType<ConductorsPage>();
            builder.RegisterType<ConductorPage>();
            builder.RegisterType<MeasurementsPage>();

            // "Exclusion Zones"
            builder.RegisterType<ExclusionZonesPage>();

            // "Minimum Approach Distances"
            builder.RegisterType<MADPage>();
            builder.RegisterType<CategoriesPage>();

            // "Working Load Limits"
            builder.RegisterType<WLLPage>();

            // "Plant & Vehicle Approach Distances"
            builder.RegisterType<PAVPage>();
            builder.RegisterType<ApprTypesPage>();
            builder.RegisterType<ObserverStatesPage>();
        }
    }
}