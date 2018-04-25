using System.Collections.Generic;
using Autofac;
using Autofac.Core;

namespace PQLines.Bootstrapping.Extensions
{   
    public static class ContainerExtensions
    {
        // This extension simply replaces the functionality of resolving a container scope object that has one or many named parameters
        // Using this should look "tidier" than the alternative way of manually calling "new NamedParameter" multiple times throughout the app
        // With reference to: http://stackoverflow.com/a/17054237

        public static T ResolveWithParameters<T>(this ILifetimeScope lifetimeScope, IDictionary<string, object> parameters)
        {
            var _parameters = new List<Parameter>();

            foreach (var parameter in parameters)
            {
                _parameters.Add(new NamedParameter(parameter.Key, parameter.Value));
            }

            return lifetimeScope.Resolve<T>(_parameters);

        }
    }
}