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
                  //.Include("~/Scripts/libraries/date.min.js")
                  //.Include("~/Scripts/libraries/daterangepicker.js")

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



    }

    /// <summary>
    /// My Extensions class.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
             "Microsoft.StyleCop.CSharp.MaintainabilityRules",
             "SA1402:FileMayOnlyContainASingleClass",
             Justification = "Splitting this file into classes would get too confusing.")]
    internal static class MyExtensions
    {
        public static void AddAndInclude(this BundleCollection bundles, string bundleVirtualPath, params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new CssTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }

        public static string RelativeFromAbsolutePath(this HttpContextBase context, string path)
        {
            var request = context.Request;
            var applicationPath = request.PhysicalApplicationPath;
            var virtualDir = request.ApplicationPath;
            virtualDir = virtualDir == "/" ? virtualDir : (virtualDir + "/");
            return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
        }
    }

    #region StyleImagePathBundle (use this when StyleBundle does not resolve images)

    /// <summary>
    /// Style Image Path Bundle (use this when StyleBundle does not resolve images).
    /// </summary>
    internal class StyleImagePathBundle : Bundle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StyleImagePathBundle" /> class.
        /// </summary>
        /// <param name="virtualPath">Virtual Path.</param>
        public StyleImagePathBundle(string virtualPath) : base(virtualPath)
        {
            Transforms.Add(new StyleRelativePathTransform());
            Transforms.Add(new CssMinify());
        }
    }

    /// <summary>
    /// Style Relative Path Transform class.
    /// </summary>
    internal class StyleRelativePathTransform : IBundleTransform
    {
        /// <summary>
        /// Function Process.
        /// </summary>
        /// <param name="context">Bundle Context.</param>
        /// <param name="response">Bundle Response.</param>
        public void Process(BundleContext context, BundleResponse response)
        {
            response.Content = string.Empty;

            var pattern = new Regex(@"url\s*\(\s*([""']?)([^:)]+)\1\s*\)", RegexOptions.IgnoreCase);
            //// open each of the files
            foreach (FileInfo cssFileInfo in response.Files)
            {
                if (cssFileInfo.Exists)
                {
                    // apply the RegEx to the file (to change relative paths)
                    string contents = File.ReadAllText(cssFileInfo.FullName);
                    MatchCollection matches = pattern.Matches(contents);

                    // Ignore the file if no match
                    if (matches.Count > 0)
                    {
                        string cssFilePath = cssFileInfo.DirectoryName;
                        string cssVirtualPath = context.HttpContext.RelativeFromAbsolutePath(cssFilePath);
                        foreach (Match match in matches)
                        {
                            // this is a path that is relative to the CSS file
                            string relativeToCSS = match.Groups[2].Value;

                            // combine the relative path to the cssAbsolute
                            string absoluteToUrl = Path.GetFullPath(Path.Combine(cssFilePath, relativeToCSS));

                            // make this server relative
                            string serverRelativeUrl = context.HttpContext.RelativeFromAbsolutePath(absoluteToUrl);

                            string quote = match.Groups[1].Value;
                            string replace = string.Format("url({0}{1}{0})", quote, serverRelativeUrl);
                            contents = contents.Replace(match.Groups[0].Value, replace);
                        }
                    }

                    // copy the result into the response.
                    response.Content = string.Format("{0}\r\n{1}", response.Content, contents);
                }
            }
        }
    }

    #endregion StyleImagePathBundle (use this when StyleBundle does not resolve images)
}