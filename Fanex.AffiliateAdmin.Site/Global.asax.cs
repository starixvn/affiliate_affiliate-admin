// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Site
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Core.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Site.App_Start;
    using Fanex.Data.Configuration;
    using Fanex.Logging;

    /// <summary>
    /// Global configuration.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application start.
        /// </summary>
        protected void Application_Start()
        {
            // Start setting connect to DB
            DbSettingProviderManager.Current.Start();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //// Register my types, for instance:
            SimpleInjectorInitializer.Initialize();
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Application Error.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event Arguments.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var currentPath = HttpContext.Current.Request.Url.AbsolutePath.ToLower();

            ////Send error email to list user in bcc email
            var message = exception.Message + Environment.NewLine + exception.StackTrace;
            LogService.Error("Affiliate Admin " + AppSettings.AffiliateName + System.Environment.NewLine + message);

            if (exception != null)
            {
                throw exception;
            }
        }

        /// <summary>
        /// Handle Begin Request.
        /// </summary>
        protected void Application_BeginRequest()
        {
            ////if (Request.IsLocal)
            ////{
            ////    MiniProfiler.Start();
            ////}
        }
       
        /// <summary>
        /// Handle End Request.
        /// </summary>
        protected void Application_EndRequest()
        {
            ////if (Request.IsLocal)
            ////{
            ////    MiniProfiler.Stop();
            ////}
        }
    }
}