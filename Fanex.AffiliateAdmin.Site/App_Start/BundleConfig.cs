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
    using System.Web;
    using System.Web.Optimization;
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    public static class BundleConfig
    {
        /// <summary>
        /// Register Bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            GetBundleForJquery(bundles);

            GetBundleForHeadJs(bundles);

            GetBundleForLogin(bundles);

            GetBundleForNotify(bundles);

            GetBundleForCommonCss(bundles);

            GetBundleForCommonJs(bundles);

            GetBundleForDataRangeJs(bundles);

            GetBundleForEachScriptFile(bundles);

            GetBundleForCommissionGroup(bundles);

            GetBundleForCompanyWinLossJs(bundles);
        }

        /// <summary>
        /// Get bundle for login.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void GetBundleForLogin(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/bundles/login")
                    .Include(
                        string.Format("~/Content/_Sites/{0}/Login.less", AppSettings.Layout),
                        "~/Content/themes/base/AffiliateAdmin.css"));

            bundles.Add(new ScriptBundle("~/bundles/loginJs").Include(
                "~/Scripts/libraries/jquery.placeholder.js",
                "~/Scripts/site/_Global.js",
                "~/Scripts/site/Login.js"));
        }

        private static void GetBundleForNotify(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/bundles/notify")
                .Include(string.Format("~/Content/_Sites/{0}/Notify.less", AppSettings.Layout)));
        }

        /// <summary>
        /// Get bundle for head.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void GetBundleForHeadJs(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/headjs")
                .Include("~/Scripts/libraries/head.js"));
        }

        /// <summary>
        /// Get bundle for head.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void GetBundleForCompanyWinLossJs(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/companywinloss")
                .Include("~/Scripts/site/CompanyWinlossByOverview.js")
                .Include("~/Scripts/site/CompanyWinlossByProduct.js")
                .Include("~/Scripts/site/CompanyWinlossReport.js"));
        }

        /// <summary>
        /// Get bundle for head.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void GetBundleForJquery(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                            .Include(
                            "~/Scripts/libraries/jquery-1.10.1.js",
                            "~/Scripts/libraries/jquery-ui-1.10.3.js",
                            "~/Scripts/libraries/jquery.blockUI.js"));
        }

        /// <summary>
        /// Get bundle for common style.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void GetBundleForCommonCss(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/bundles/css")
                            .Include(
                                "~/Content/themes/base/jquery.ui.core.css",
                                "~/Content/themes/base/jquery.ui.resizable.css",
                                "~/Content/themes/base/jquery.ui.selectable.css",
                                "~/Content/themes/base/jquery.ui.accordion.css",
                                "~/Content/themes/base/jquery.ui.autocomplete.css",
                                "~/Content/themes/base/jquery.ui.button.css",
                                "~/Content/themes/base/jquery.ui.dialog.css",
                                "~/Content/themes/base/jquery.ui.slider.css",
                                "~/Content/themes/base/jquery.ui.tabs.css",
                                "~/Content/themes/base/jquery.ui.datepicker.css",
                                "~/Content/themes/base/jquery.ui.progressbar.css",
                                "~/Content/themes/base/jquery.ui.theme.css",
                                "~/Content/themes/base/jquery.multiselect.css",
                                "~/Content/themes/base/daterangepicker.css",
                                "~/Content/themes/base/monpickr.css",
                                "~/Content/themes/base/Fanex.Grid.css",
                                "~/Content/themes/base/Fanex.Pagination.css",
                                "~/Content/themes/base/AffiliateAdmin.css")
                            .Include("~/Content/themes/Default/index.less"));
        }

        /// <summary>
        /// Get bundle for common java-script.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void GetBundleForCommonJs(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js")
                            .Include(
                                "~/Scripts/libraries/moment.js",
                                "~/Scripts/libraries/numeral.js",
                                "~/Scripts/libraries/jquery.placeholder.js",
                                "~/Scripts/libraries/jquery.mtz.monthpicker.seph.custom.js",
                                "~/Scripts/libraries/jquery.multiselect.js",
                                "~/Scripts/libraries/jquery.formattyping.js",
                                "~/Scripts/libraries/jquery.numeric.js",
                                "~/Scripts/libraries/jquery.ba-resize.min.js",
                                "~/Scripts/libraries/Fanex.Grid.js",
                                "~/Scripts/libraries/Fanex.Pagination.js",
                                "~/Scripts/libraries/Fanex.Grid.Convert.js")
                            .IncludeDirectory("~/Scripts/libraries/Language", "Fanex*")
                            .Include(
                                  "~/Scripts/libraries/head.js",
                                  "~/Scripts/site/Query.string.js",
                                  "~/Scripts/site/Paging.js",
                                  "~/Scripts/site/_Global.js",
                                  "~/Scripts/site/RecheckIn.js"));
        }

        /// <summary>
        /// Get bundle for data range.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void GetBundleForDataRangeJs(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/daterange")
            .Include(
                    "~/Scripts/libraries/date.min.js",
                    "~/Scripts/libraries/daterangepicker.js",
                    "~/Scripts/libraries/monpickr.js"));
        }

        /// <summary>
        /// Get bundle for each script file.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void GetBundleForEachScriptFile(BundleCollection bundles)
        {
            const string RelativeScriptPath = "/Scripts/site";
            var absoluteScriptPath = HttpRuntime.AppDomainAppPath + RelativeScriptPath;
            var scriptDir = new DirectoryInfo(absoluteScriptPath);
            var scriptFiles = scriptDir.GetFiles("*.js");

            foreach (var scriptFile in scriptFiles)
            {
                var fileName = scriptFile.Name;
                var fileNameWithouExtension = fileName.Substring(0, fileName.LastIndexOf(scriptFile.Extension, System.StringComparison.OrdinalIgnoreCase));

                var bundle = new ScriptBundle(string.Format("~/scripts/{0}", fileNameWithouExtension.ToLower()));
                bundle.Include(string.Format("~{0}/{1}", RelativeScriptPath, fileName));
                bundles.Add(bundle);
            }
        }

        /// <summary>
        /// Get bundle for each script file.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void GetBundleForCommissionGroup(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/bundles/commgroup")
                .Include("~/Content/themes/Default/commgroup.less"));
        }
    }
}
