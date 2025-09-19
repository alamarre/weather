using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace Weather.Api.Infrastructure
{
    public class SimpleDependencyResolver : IDependencyResolver
    {
        private readonly IDictionary<Type, Func<object>> _registrations;

        public SimpleDependencyResolver(IDictionary<Type, Func<object>> registrations)
        {
            _registrations = registrations ?? throw new ArgumentNullException(nameof(registrations));
        }

        public object GetService(Type serviceType)
        {
            return _registrations.TryGetValue(serviceType, out var factory)
                ? factory()
                : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var service = GetService(serviceType);
            if (service == null)
            {
                return Array.Empty<object>();
            }

            return new[] { service };
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
