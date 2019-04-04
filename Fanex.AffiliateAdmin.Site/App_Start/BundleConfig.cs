namespace Fanex.AffiliateAdmin.Site
{
    using System.IO;
    using System.Web;
    using System.Web.Optimization;

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

            GetBundleForCommonJs(bundles);

            GetBundleForDataRangeJs(bundles);

            GetBundleForEachScriptFile(bundles);

            GetBundleForCompanyWinLossJs(bundles);

            GetBundleForAffiliateWinLossJs(bundles);

            GetBundleForMemberWinLossJs(bundles);

            GetBundleForAffiliateCommissionJs(bundles);

            GetBundleForBetList(bundles);

            GetBundleForToolTips(bundles);
        }

        /// <summary>
        /// Get bundle for login.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void GetBundleForLogin(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/loginJs").Include(
                "~/Scripts/libraries/cryptoJS.js",
                "~/Scripts/libraries/jquery.placeholder.js",
                "~/Scripts/libraries/jquery.blockUI.js",
                "~/Scripts/site/Login.js"));
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
                .Include("~/Scripts/site/togglemanager.js")
                .Include("~/Scripts/site/companywinlossbyoverview.js")
                .Include("~/Scripts/site/companywinlossbyproduct.js")
                .Include("~/Scripts/site/companywinlossreport.js"));
        }

        private static void GetBundleForMemberWinLossJs(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/memberwinloss")
                .Include("~/Scripts/site/stopwatch.js")
                .Include("~/Scripts/site/togglemanager.js")
                .Include("~/Scripts/site/memberwinlossbyoverview.js")
                .Include("~/Scripts/site/memberwinlossbyproduct.js")
                .Include("~/Scripts/site/memberwinlossreport.js"));
        }

        private static void GetBundleForAffiliateWinLossJs(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/affiliatewinloss")
                .Include("~/Scripts/site/togglemanager.js")
                .Include("~/Scripts/site/affiliatewinlossbyoverview.js")
                .Include("~/Scripts/site/affiliatewinlossbyproduct.js")
                .Include("~/Scripts/site/affiliatewinlossreport.js"));
        }

        private static void GetBundleForAffiliateCommissionJs(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/affiliatecommission")
                            .Include("~/Scripts/site/togglemanager.js")
                .Include("~/Scripts/site/affiliatecommissionbyproduct.js")
                .Include("~/Scripts/site/affiliatecommissionreport.js"));
        }

        /// <summary>
        /// Get bundle for head.
        /// </summary>
        /// <param name="bundles">Bundle collection.</param>
        private static void GetBundleForJquery(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                            .Include(
                            "~/Scripts/libraries/jquery-1.11.3.min.js",
                            "~/Scripts/libraries/jquery-ui.min.js",
                            "~/Scripts/libraries/jquery.blockUI.js"));
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
                                  "~/Scripts/site/_Global.js"));
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
        /// Get Bundle for Bet List.
        /// </summary>
        /// <param name="bundles">Bundle input.</param>
        private static void GetBundleForBetList(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/betlist")
               .Include("~/Content/betlist/global.content.css")
               .Include("~/Content/betlist/betlist.core.css")
               .Include("~/Content/betlist/betlist.css")
               .Include("~/Content/betlist/systemparlay.detail.css")
               .Include("~/Content/betlist/rngcasino.handhistory.css")
               .Include("~/Content/betlist/fanex.result.css")
               .Include("~/Content/betlist/custom-betlist.css"));

            bundles.Add(new Bundle("~/js/betlist")
                .Include("~/Scripts/site/betlist.js"));

            bundles.Add(new ScriptBundle("~/js/betlistrngcasino")
                .Include("~/Scripts/site/rngcasino-results.js"));

            bundles.Add(new LessBundle("~/css/systemparlaydetails")

           .Include("~/Content/betlist/global.content.css")
           .Include("~/Content/betlist/systemparlay.detail.css"));
        }

        private static void GetBundleForToolTips(BundleCollection bundles)
        {
            bundles.Add(new LessBundle("~/css/tooltip")
                        .Include("~/Content/tipsy/tipsy.css")
                        .Include("~/Content/tipsy/custom-tipsy.css"));

            bundles.Add(new ScriptBundle("~/js/tooltip")
                .Include("~/Scripts/libraries/jquery.tipsy.js"));
        }
    }
}