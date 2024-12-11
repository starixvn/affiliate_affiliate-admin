namespace Fanex.AffiliateAdmin.Site
{
    using Core.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Helpers;
    using Fanex.Data;
    using Fanex.Data.SqlServer;
    using System;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    /// <summary>
    /// Global configuration.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Application start.
        /// </summary>
        protected void Application_Start()
        {
            LoggingConfig.RegisterSentry();
            // Start setting connect to DB
            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer("~/Configurations/SpNames/SpName.xml")
                .Run();
            UMConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //// Register my types, for instance:
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());

            BundleTable.EnableOptimizations = AppSettings.GetValue("DisableOptimizations") != "1";
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }

        /// <summary>
        /// Application Error.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event Arguments.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            if (exception is HttpException httpException && httpException.GetHttpCode() == 404)
            {
                return;
            }

            LogHelper.WriteError(exception);
        }
    }
}