using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Integration.Mvc;
using System;

namespace Fanex.AffiliateAdmin.Controller.UnitTests.Helpers
{
    public class SimpleLifetimeScopeProvider : ILifetimeScopeProvider
    {
        private readonly IContainer _container;
        private ILifetimeScope _scope;

        public SimpleLifetimeScopeProvider(IContainer container)
        {
            _container = container;
        }

        public ILifetimeScope ApplicationContainer
        {
            get { return _container; }
        }

        public void EndLifetimeScope()
        {
            if (_scope != null)
            {
                _scope.Dispose();
                _scope = null;
            }
        }

        public ILifetimeScope GetLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            if (_scope == null)
            {
                _scope = (configurationAction == null)
                       ? ApplicationContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag)
                       : ApplicationContainer.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag, configurationAction);
            }

            return _scope;
        }
    }
}
