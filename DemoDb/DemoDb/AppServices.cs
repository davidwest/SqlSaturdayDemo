using System;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace DemoDb
{
    public static class AppServices
    {
        private static bool _isInitialized;
        private static Container _container;

        public static void Initialize(Container container)
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException("Already initialized");
            }

            _container = container;
            _isInitialized = true;
        }
        
        public static TService Get<TService>() where TService : class
        {
            return _container.GetInstance<TService>();
        }

        public static void Use<TService>(Action<TService> use) where TService : class
        {
            using (ThreadScopedLifestyle.BeginScope(_container))
            {
                var service = _container.GetInstance<TService>();

                use(service);
            }
        }
    }
}
