namespace Fanex.AffiliateAdmin.Core.Mvc
{
    using System.Threading;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// Custom Razor View Engine.
    /// </summary>
    public class CustomRazorViewEngine : RazorViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomRazorViewEngine"/> class.
        /// </summary>
        public CustomRazorViewEngine()
        {
            this.ViewLocationFormats = new string[] 
                                                {
                                                 "~/Views/Layout%1/{1}/{0}.cshtml",
                                                 "~/Views/Layout%1/{1}/{0}.vbhtml",
                                               };

            this.MasterLocationFormats = new string[] 
                                                    {
                                                   "~/Views/Layout%1/{1}/{0}.cshtml",
                                                   "~/Views/Layout%1/{1}/{0}.vbhtml",
                                                   };

            this.PartialViewLocationFormats = new string[]
                                                    {
                                                        "~/Views/Layout%1/{1}/{0}.cshtml",
                                                        "~/Views/Layout%1/{1}/{0}.vbhtml",
                                                        "~/Views/Layout%1/Shared/{0}.cshtml",
                                                        "~/Views/Layout%1/Shared/{0}.vbhtml"
                                                    };

            this.FileExtensions = new string[] { "cshtml", "vbhtml" };
        }

        /// <summary>
        /// Find a View.
        /// </summary>
        /// <param name="controllerContext">Controller Context.</param>
        /// <param name="viewName">View Name.</param>
        /// <param name="masterName">Master Name.</param>
        /// <param name="useCache">Use Cache.</param>
        /// <returns>Return relevant view.</returns>
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var localizedViewName = viewName;
            if (!string.IsNullOrEmpty(viewName))
            {
                localizedViewName += string.Format("{0}{1}", ".", Thread.CurrentThread.CurrentUICulture.Name);
            }

            var localizedMasterName = masterName;
            if (!string.IsNullOrEmpty(masterName))
            {
                localizedMasterName += string.Format("{0}{1}", ".", Thread.CurrentThread.CurrentUICulture.Name);
            }

            var result = base.FindView(controllerContext, localizedViewName, localizedMasterName, useCache);
            if (result.View == null)
            {
                result = base.FindView(controllerContext, viewName, masterName, useCache);
            }

            return result;
        }

        /// <summary>
        /// Create View.
        /// </summary>
        /// <param name="controllerContext">Controller Context.</param>
        /// <param name="viewPath">View Path.</param>
        /// <param name="masterPath">Master Path.</param>
        /// <returns>Return View.</returns>
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return base.CreateView(controllerContext, viewPath.Replace("%1", AppSettings.Layout), masterPath);
        }

        /// <summary>
        /// Create Partial View.
        /// </summary>
        /// <param name="controllerContext">Controller Context.</param>
        /// <param name="partialPath">Partial Path.</param>
        /// <returns>Return Partial view.</returns>
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return base.CreatePartialView(controllerContext, partialPath.Replace("%1", AppSettings.Layout));
        }

        /// <summary>
        /// Check file Exists.
        /// </summary>
        /// <param name="controllerContext">Controller Context.</param>
        /// <param name="virtualPath">Virtual Path.</param>
        /// <returns>Return File exists or not.</returns>
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return base.FileExists(controllerContext, virtualPath.Replace("%1", AppSettings.Layout));
        }
    }
}