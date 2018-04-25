using Autofac;
using PQLines.ViewModels;
using PQLines.ViewModels.ConductorInfo;
using PQLines.ViewModels.ConductorInfo.Conductors;
using PQLines.ViewModels.ConductorInfo.Conductors.Measurements;
using PQLines.ViewModels.ExclusionZones;
using PQLines.ViewModels.LiveLineMinApprDist.Categories;
using PQLines.ViewModels.PlantAndVehicleApprDist;
using PQLines.ViewModels.PlantAndVehicleApprDist.ApprTypes;
using PQLines.ViewModels.PlantAndVehicleApprDist.ApprTypes.ObserverStates;

namespace PQLines.Bootstrapping.Registration
{
    public class ViewModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register all View Models
            // (HomePageViewModel could probably be registered as SingleInstance)

            builder.RegisterType<HomePageViewModel>();

            // "Conductor Info"
            builder.RegisterType<ConductorsViewModel>();
            builder.RegisterType<ConductorViewModel>();
            builder.RegisterType<MeasurementsViewModel>();

            // "Exclusion Zones"
            builder.RegisterType<ExclusionZonesViewModel>();

            // "Minimum Approach Distances"
            builder.RegisterType<MADViewModel>();
            builder.RegisterType<CategoriesViewModel>();

            // "Working Load Limits"
            builder.RegisterType<WLLViewModel>();

            // "Plant & Vehicle Approach Distances"
            builder.RegisterType<PAVViewModel>();
            builder.RegisterType<ApprTypesViewModel>();
            builder.RegisterType<ObserverStatesViewModel>();
        }
    }
}