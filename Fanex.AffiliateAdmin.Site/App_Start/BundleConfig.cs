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
    using BundleTransformer.Core.Transformers;
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    public class BundleConfig
    {
        private static readonly CssMinify _cssMinify = new CssMinify();
        private static readonly CssTransformer _cssTransformer = new CssTransformer();
        private static readonly JsMinify _jsMinify = new JsMinify();
        private static readonly JsTransformer _jsTransformer = new JsTransformer();

        /// <summary>
        /// Register Bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(GetBundleForLoginCss());

            bundles.Add(GetBundleForNotifyCss());

            bundles.Add(GetBundleForLoginJs());

            bundles.Add(GetBundleForHeadJs());

            bundles.Add(GetBundleForCommonCss());   
         
            bundles.Add(GetBundleForCommonJs());

            bundles.Add(GetBundleForDataRangeJs());

            GetBundleForEachScriptFile(bundles);
        }

        private static Bundle GetBundleForDataRangeJs()
        {
            var bundle = new ScriptBundle("~/bundles/daterange");

            AddTransformerForJavascript(bundle);

            bundle.Include("~/Scripts/libraries/date.min.js")
                  .Include("~/Scripts/libraries/daterangepicker.js")
                  .Include("~/Scripts/libraries/monpickr.js");

            return bundle;
        }

        private static Bundle GetBundleForCommonJs()
        {
            var bundle = new ScriptBundle("~/bundles/js");

            AddTransformerForJavascript(bundle);

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
            var bundle = new Bundle("~/bundles/css");

            AddTransformerForCss(bundle);

            bundle.IncludeDirectory("~/Content/themes/base", "*.css")
                  .Include("~/Content/themes/Default/index.less");
                 
            return bundle;
        }

        private static Bundle GetBundleForHeadJs()
        {
            var bundle = new ScriptBundle("~/bundles/headjs");

            AddTransformerForJavascript(bundle);

            bundle.Include("~/Scripts/libraries/head.js");

            return bundle;
        }

        private static Bundle GetBundleForLoginJs()
        {
            var bundle = new ScriptBundle("~/bundles/loginJs");

            AddTransformerForJavascript(bundle);

            bundle.Include("~/Scripts/libraries/jquery.blockUI.js")
                  .Include("~/Scripts/libraries/jquery.placeholder.js")
                  .Include("~/Scripts/site/Login.js")
                  .Include("~/Scripts/site/_Global.js");

            return bundle;
        }

        private static Bundle GetBundleForNotifyCss()
        {
            var bundle = new Bundle("~/bundles/notify");

            AddTransformerForCss(bundle);

            bundle.Include(string.Format("~/Content/_Sites/{0}/Notify.less", AppSettings.Layout));

            return bundle;
        }

        private static Bundle GetBundleForLoginCss()
        {
            var bundle = new Bundle("~/bundles/login");

            AddTransformerForCss(bundle);
            
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

                AddTransformerForJavascript(bundle);

                bundle.Include(string.Format("~{0}/{1}", relativeScriptPath, fileName));
                bundles.Add(bundle);
            }
        }

        private static void AddTransformerForCss(Bundle bundle)
        {
            if (AppSettings.EnableTransformer)
            {
                bundle.Transforms.Add(_cssMinify);
                bundle.Transforms.Add(_cssTransformer);
            }
        }

        private static void AddTransformerForJavascript(Bundle bundle)
        {
            if (AppSettings.EnableTransformer)
            {
                bundle.Transforms.Add(_jsMinify);
                bundle.Transforms.Add(_jsTransformer);
            }
        }
    }
}