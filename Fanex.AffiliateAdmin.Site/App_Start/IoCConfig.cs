using Autofac;
using Autofac.Integration.Mvc;
using Fanex.AffiliateAdmin.Core.Utilities;
using Fanex.Caching;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.NETFramework;
using ONEbook.UM.Transport;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;

namespace Fanex.AffiliateAdmin.Site
{
    public static class IoCConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            //Register repository
            var asselmblyList = GetExecutingAssembly();

            builder.RegisterAssemblyTypes(asselmblyList).AsImplementedInterfaces();

            builder.RegisterDependencies();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(asselmblyList);


            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static Assembly[] GetExecutingAssembly()
        {
            Assembly[] assemblies = null;
            try
            {
                assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            }
            catch
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(i => !i.IsDynamic).ToArray();
            }

            return assemblies.Where(x => x.FullName.StartsWith("Fanex.AffiliateAdmin", StringComparison.InvariantCulture)).ToArray();
        }

        private static void RegisterDependencies(this ContainerBuilder builder)
        {
            builder.RegisterInstance(new UMClientConfiguration().Initialize());
            builder.RegisterInstance<ICacheService>(new CacheService());

            builder.RegisterType<UMCacheService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<IPHelperImpl>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<UMRestClient>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<HttpContextHelper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}