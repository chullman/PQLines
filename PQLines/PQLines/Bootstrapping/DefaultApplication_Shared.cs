using Autofac;
using PQLines.Bootstrapping.Registration;

namespace PQLines.Bootstrapping
{
    public class DefaultApplication_Shared : IApplication<ApplicationRuntime_Shared>
    {
        // Bootstrap the Autofac container
        // This container holds all services, View Models, and Views in this project

        public ApplicationRuntime_Shared Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new StartupServicesModule());
            builder.RegisterModule(new ViewModelsModule());
            builder.RegisterModule(new ViewModule());

            var nativeContainer = builder.Build();

            return new ApplicationRuntime_Shared(nativeContainer);
        }

        public static DefaultApplication_Shared Build()
        {
            return new DefaultApplication_Shared();
        }
    }
}