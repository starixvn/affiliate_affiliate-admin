// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Site
{
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    public class BundleConfig
    {
        private static CssMinify _cssMinify = new CssMinify();
        private static CssTransformer _cssTransformer = new CssTransformer();
        private static NullOrderer _nullOrderer = new NullOrderer();

        /// <summary>
        /// Register Bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", 
            "SP2101:MethodMustNotContainMoreLinesThan", Justification = "This is OK here.")]
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(GetBundleForLoginCss());

            bundles.Add(GetBundleForNotifyCss());

            bundles.Add(GetBundleForLoginJs());

            bundles.Add(GetBundleForHeadJs());

            bundles.Add(GetBundleForCommonCss());   
         
            bundles.Add(GetBundleForCommonJs());

            bundles.Add(new ScriptBundle("~/bundles/daterange").Include(
                "~/Scripts/libraries/date.min.js").Include(
                "~/Scripts/libraries/daterangepicker.js").Include(
                "~/Scripts/libraries/monpickr.js"));

            GetBundleForEachScriptFile(bundles);
        }

        private static Bundle GetBundleForCommonJs()
        {
            var bundle = new ScriptBundle("~/bundles/js");
            
            bundle.IncludeDirectory("~/Scripts/libraries", "jquery*.js")
                  .IncludeDirectory("~/Scripts/libraries", "Fanex*.js")
                  .IncludeDirectory("~/Scripts/libraries/Language", "*.js")
                  .Include("~/Scripts/libraries/head.js")
                  .Include("~/Scripts/libraries/moment.js")
                  .Include("~/Scripts/libraries/monpickr.js")
                  .Include("~/Scripts/libraries/numeral.js")
                  .Include("~/Scripts/libraries/placeholder_fixer.js")
                  .Include("~/Scripts/libraries/ui.dropdownchecklist.js")
                  .Include("~/Scripts/site/Query.string.js")
                  .Include("~/Scripts/site/Paging.js")
                  .Include("~/Scripts/site/_Global.js")
                  .Include("~/Scripts/site/RecheckIn.js");

            return bundle;
        }

        private static Bundle GetBundleForCommonCss()
        {
            var bundle = new Bundle("~/bundles/css")
                {
                    Transforms = { _cssTransformer, _cssMinify },
                    Orderer = _nullOrderer
                };

            bundle.IncludeDirectory("~/Content/themes/base", "*.css")
                  .Include("~/Content/themes/Default/index.less");
                 
            return bundle;
        }

        private static Bundle GetBundleForHeadJs()
        {
            return new ScriptBundle("~/bundles/headjs").Include("~/Scripts/libraries/head.js");
        }

        private static Bundle GetBundleForLoginJs()
        {
            var bundle = new ScriptBundle("~/bundles/loginJs");
            bundle.Include("~/Scripts/libraries/jquery.blockUI.js")
                  .Include("~/Scripts/libraries/jquery.placeholder.js")
                  .Include("~/Scripts/site/Login.js")
                  .Include("~/Scripts/site/_Global.js");

            return bundle;
        }

        private static Bundle GetBundleForNotifyCss()
        {
            return new Bundle("~/bundles/notify")
                    .Include(string.Format("~/Content/_Sites/{0}/Notify.less", AppSettings.Layout));
        }

        private static Bundle GetBundleForLoginCss()
        {
            var bundle = new Bundle("~/bundles/login");
            bundle.Transforms.Add(_cssTransformer);
            bundle.Transforms.Add(_cssMinify);
            bundle.Orderer = _nullOrderer;
            
            bundle.Include(string.Format("~/Content/_Sites/{0}/Login.less", AppSettings.Layout))
                  .Include("~/Content/themes/base/AffiliateAdmin.css");

            return bundle;
        }

        private static void GetBundleForEachScriptFile(BundleCollection bundles)
        {
            const string relativeScriptPath = "/Scripts/site";
            var absoluteScriptPath = HttpRuntime.AppDomainAppPath + relativeScriptPath;
            var scriptDir = new DirectoryInfo(absoluteScriptPath);
            var scriptFiles = scriptDir.GetFiles("*.js");

            foreach (var scriptFile in scriptFiles)
            {
                var fileName = scriptFile.Name;
                var fileNameWithouExtension = fileName.Substring(0, fileName.LastIndexOf(scriptFile.Extension, System.StringComparison.OrdinalIgnoreCase));

                var bundle = new Bundle(string.Format("~/scripts/{0}", fileNameWithouExtension.ToLower()));
                bundle.Include(string.Format("~{0}/{1}", relativeScriptPath, fileName));
                bundles.Add(bundle);
            }
        }
    }
}