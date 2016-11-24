namespace Fanex.AffiliateAdmin.Site
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using CasinoSetting;
    using Core.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Helpers;
    using Fanex.Data.Configuration;

    /// <summary>
    /// Global configuration.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application start.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Allow underscore")]
        protected void Application_Start()
        {
            // Start setting connect to DB
            DbSettingProviderManager.Current.Start();

            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //// Register my types, for instance:
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());

            BundleTable.EnableOptimizations = AppSettings.GetValue("DisableOptimizations") != "1";
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();
            log4net.LogManager.GetLogger("GeneralLog").Info("Start");

            if (AppSettings.Environment.IndexOf("Local", StringComparison.OrdinalIgnoreCase) < 0 &&
                !AppSettings.Environment.Equals("SIT", StringComparison.OrdinalIgnoreCase))
            {
                SettingMgr.GetInstance().Initialize(SiteEnum.AgentSite);
            }
        }

        /// <summary>
        /// Application Error.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event Arguments.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            var httpException = exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                return;
            }

            LogHelper.WriteError(exception);
        }
    }
}