using System;
using Autofac;

namespace PQLines.Bootstrapping
{
    // Currently for runtime management of the Autofac container (e.g. storage and disposal)

    public class ApplicationRuntime_Shared : IDisposable
    {
        public ApplicationRuntime_Shared(IContainer container)
        {
            Container = container;
        }

        public IContainer Container { get; private set; }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}