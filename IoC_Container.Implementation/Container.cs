using System;
using System.Collections.Generic;
using System.Linq;

namespace IoC_Container.Implementation
{
    public class Container
    {
        /// <summary>
        /// Container for the services and implementations
        /// </summary>
        private Dictionary<Type, Func<object>> _registers = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// Register service in the IoC container
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <typeparam name="TImplementation">Service implementation</typeparam>
        public void Register<TService, TImplementation>() where TImplementation : TService
        {
            _registers.Add(typeof(TService), () => GetInstance(typeof(TImplementation)));
        }

        /// <summary>
        /// Register service in the IoC container
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <param name="instanceCreator">Creator of the service</param>
        public void Register<TService>(Func<TService> instanceCreator)
        {
            _registers.Add(typeof(TService), () => instanceCreator);
        }

        /// <summary>
        /// Register service in the IoC container as a singleton
        /// </summary>
        /// <typeparam name="TService">Service type</typeparam>
        /// <param name="service">Service instance</param>
        public void RegisterSingleton<TService>(TService service)
        {
            _registers.Add(typeof(TService), () => service);
        }

        /// <summary>
        /// Get instance of a registered service
        /// </summary>
        /// <param name="serviceType">Service type</param>
        /// <returns>Instance of the service</returns>
        public object GetInstance(Type serviceType)
        {
            Func<object> implementation;

            // Get instance of the service...
            if (_registers.TryGetValue(serviceType, out implementation))
            {
                // ...if implementation is in the container already, return service implementation
                return implementation;
            }
            else if (!serviceType.IsAbstract)
            {
                // ...if implementation is not in the container, create it's instance and return it
                return CreateInstance(serviceType);
            }
            else
            {
                // ..if given service is not registered, throw exception
                throw new InvalidOperationException($"Service {serviceType} not registered!");
            }
        }

        /// <summary>
        /// Create instance of a service if its possible (service is not an abstraction of any kind)
        /// </summary>
        /// <param name="implementationType">Type of service implementation</param>
        /// <returns>Instance of the service</returns>
        private object CreateInstance(Type implementationType)
        {
            // Get constructor of the registered implementation
            var constructor = implementationType.GetConstructors()[0];
            // Get constructor parameters
            var ctorParams = constructor.GetParameters();
            // Get constructor parameters types
            var ctorParamsTypes = ctorParams.Select(p => p.ParameterType);
            // Get constructor parameter dependencies
            var dependencies = ctorParamsTypes.Select(p => GetInstance(p)).ToList();

            // Return new instance of the service
            return Activator.CreateInstance(implementationType, dependencies);
        }
    }
}
